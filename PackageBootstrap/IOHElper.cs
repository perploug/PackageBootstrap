// -----------------------------------------------------------------------
// <copyright file="IOHElper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PackageBootstrap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IOHelper
    {
        public static string MapPath(string path, bool useHttpContext, string basedirectory)
        {
            // Check if the path is already mapped
            if (path.Length >= 2 && path[1] == Path.VolumeSeparatorChar)
                return path;

            if (useHttpContext)
            {
                //string retval;
                if (!string.IsNullOrEmpty(path) && (path.StartsWith("~")))
                    return System.Web.Hosting.HostingEnvironment.MapPath(path);
                else
                    return System.Web.Hosting.HostingEnvironment.MapPath("~/" + path.TrimStart('/'));
            }
            else
            {
                string _root;

                if (!string.IsNullOrEmpty(basedirectory))
                    _root = basedirectory;
                else
                    _root = basedirectory;


                string _path = path.TrimStart('~', '/').Replace('/', DirSepChar);
                string retval = _root + DirSepChar.ToString() + _path;
                return retval;
            }
        }

        public static char DirSepChar
        {
            get
            {
                return Path.DirectorySeparatorChar;
            }
        }
        
    }
}
