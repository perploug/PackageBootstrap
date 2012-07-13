using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PackageBootstrap.Interfaces;

namespace PackageBootstrap
{
    public class TaskmanagerEventArgs : EventArgs
    {
        public ITask Task { get; set; }
        public Exception Exception { get; set; }

        public bool HasException
        {
            get
            {
                return Exception != null;
            }
        }
    }
}