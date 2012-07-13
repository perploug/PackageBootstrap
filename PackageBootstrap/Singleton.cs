using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageBootstrap
{
    public sealed class Singleton<T> where T : new()
    {
		#region Constructors (1) 

        private Singleton()
        {
        }

		#endregion Constructors 

		#region Properties (1) 

        public static T Instance
        {
            get { return Nested.instance; }
        }

		#endregion Properties 

		#region Nested Classes (1) 


        private class Nested
        {
		#region Fields (1) 

            internal static readonly T instance = new T();

		#endregion Fields 

		#region Constructors (1) 

            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

		#endregion Constructors 
        }
		#endregion Nested Classes 
    }
}
