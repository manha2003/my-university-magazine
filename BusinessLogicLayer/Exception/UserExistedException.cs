using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Exception
{ 
        public class UserExistedException : System.Exception
        {
            public UserExistedException() { }

            public UserExistedException(string message)
                : base(message) { }

            public UserExistedException(string message, System.Exception inner)
                : base(message, inner) { }
        }
    
}
