using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageBootstrap.Exceptions
{
    public class ProviderException : Exception
    {
		#region Constructors (1) 

        public ProviderException(string message)
            : base(message)
        {
        }

		#endregion Constructors 
    }
}
