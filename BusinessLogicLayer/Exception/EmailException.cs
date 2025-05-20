using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Exception
{
    public class EmailException : System.Exception
    {
        public EmailException() { }

        public EmailException(string message)
            : base(message) { }

        public EmailException(string message, System.Exception inner)
            : base(message, inner) { }
    }
}
