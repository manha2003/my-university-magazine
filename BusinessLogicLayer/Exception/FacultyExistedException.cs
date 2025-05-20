using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Exception
{
    public class FacultyExistedException : System.Exception
    {
        public FacultyExistedException() { }

        public FacultyExistedException(string message)
            : base(message) { }

        public FacultyExistedException(string message, System.Exception inner)
            : base(message, inner) { }
    }
}
