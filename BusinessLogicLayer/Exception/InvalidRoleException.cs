using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Exception
{
    public class InvalidRoleException : System.Exception
    {
        public InvalidRoleException() { }

        public InvalidRoleException(string message)
            : base(message) { }

        public InvalidRoleException(string message, System. Exception inner)
            : base(message, inner) { }
    }
}
