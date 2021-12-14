using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine.Exceptions
{
    public class InternalServerError : Exception
    {
        public InternalServerError()
        {
        }

        public InternalServerError(string message) : base(message)
        {
        }
    }
}
