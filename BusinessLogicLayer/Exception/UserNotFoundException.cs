using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Exception
{
   
        public class UserNotFoundException : System.Exception
        {
            public UserNotFoundException() { }

            public UserNotFoundException(string message)
                : base(message) { }

            public UserNotFoundException(string message, System.Exception inner)
                : base(message, inner) { }
        }
    
}
