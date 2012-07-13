﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace PackageBootstrap
{
    public static class TypeFinder
    {
        private static readonly Dictionary<string, List<Type>> Types = new Dictionary<string, List<Type>>();

        #region Methods (5)

        // Public Methods (3) 

        /// <summary>
        /// Searches all loaded assemblies for classes of the type passed in.
        /// </summary>
        /// <typeparam name="T">The type of object to search for</typeparam>
        /// <returns>A list of found types</returns>
        public static List<Type> FindClassesOfType<T>()
        {
            return FindClassesOfType<T>(true);
        }

        /// <summary>
        /// Searches all loaded assemblies for classes of the type passed in.
        /// </summary>
        /// <typeparam name="T">The type of object to search for</typeparam>
        /// <param name="useSeperateAppDomain">true if a seperate app domain should be used to query the types. This is safer as it is able to clear memory after loading the types.</param>
        /// <returns>A list of found types</returns>
        public static List<Type> FindClassesOfType<T>(bool useSeperateAppDomain)
        {
            return FindClassesOfType<T>(useSeperateAppDomain, true);
        }

        /// <summary>
        /// Searches all loaded assemblies for classes of the type passed in.
        /// </summary>
        /// <typeparam name="T">The type of object to search for</typeparam>
        /// <param name="useSeperateAppDomain">true if a seperate app domain should be used to query the types. This is safer as it is able to clear memory after loading the types.</param>
        /// <param name="onlyConcreteClasses">True to only return classes that can be constructed</param>
        /// <returns>A list of found types</returns>
        public static List<Type> FindClassesOfType<T>(bool useSeperateAppDomain, bool onlyConcreteClasses)
        {
            var key = typeof(T).Name + "_" + useSeperateAppDomain + "_" + onlyConcreteClasses;
            if (!Types.ContainsKey(key))
            {
                var foundClasses = FindClassesOfTypeCached<T>(useSeperateAppDomain, onlyConcreteClasses);
                Types.Add(key, foundClasses);
                return foundClasses;
            }
            return Types[key];
        }

        private static List<Type> FindClassesOfTypeCached<T>(bool useSeperateAppDomain, bool onlyConcreteClasses)
        {
            if (useSeperateAppDomain)
            {
                string binFolder = Path.Combine(IOHelper.MapPath("/", false, string.Empty), "bin");
                string[] strTypes = TypeResolver.GetAssignablesFromType<T>(binFolder, "*.dll");


                List<Type> types = new List<Type>();

                foreach (string type in strTypes)
                    types.Add(Type.GetType(type));

                // also add types from app_code
                try
                {
                    // only search the App_Code folder if it's not empty
                    DirectoryInfo appCodeFolder = new DirectoryInfo(IOHelper.MapPath("~/App_code", false, string.Empty));
                    if (appCodeFolder.GetFiles().Length > 0)
                    {
                        foreach (Type type in System.Reflection.Assembly.Load("App_Code").GetTypes())
                            types.Add(type);
                    }
                }
                catch { } // Empty catch - this just means that an App_Code assembly wasn't generated by the files in folder

                return types.FindAll(OnlyConcreteClasses(typeof(T), onlyConcreteClasses));
            }
            else
                return FindClassesOfType(typeof(T), onlyConcreteClasses);
        }
        // Private Methods (2) 

        /// <summary>
        /// Finds all classes with the specified type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A list of found classes</returns>
        private static List<Type> FindClassesOfType(Type type, bool onlyConcreteClasses)
        {
            bool searchGAC = false;

            List<Type> classesOfType = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsDynamic)
                {
                    //don't check any types if the assembly is part of the GAC
                    if (!searchGAC && assembly.GlobalAssemblyCache)
                        continue;

                    Type[] publicTypes;
                    //need try catch block as some assemblies do not support this (i.e.RefEmit_InMemoryManifestModule)
                    try
                    {
                        publicTypes = assembly.GetExportedTypes();
                    }
                    catch
                    {
                        continue;
                    }

                    List<Type> listOfTypes = new List<Type>();
                    listOfTypes.AddRange(publicTypes);
                    List<Type> foundTypes = listOfTypes.FindAll(OnlyConcreteClasses(type, onlyConcreteClasses));
                    Type[] outputTypes = new Type[foundTypes.Count];
                    foundTypes.CopyTo(outputTypes);
                    classesOfType.AddRange(outputTypes);
                }
            }

            return classesOfType;
        }

        private static Predicate<Type> OnlyConcreteClasses(Type type, bool onlyConcreteClasses)
        {
            return t => (type.IsAssignableFrom(t) && (onlyConcreteClasses ? (t.IsClass && !t.IsAbstract) : true));
        }

        #endregion Methods
    }
}
