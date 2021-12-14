using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine.Exceptions
{
    public class MissingMandatoryInformationException : Exception
    {
        public MissingMandatoryInformationException()
        {
        }

        public MissingMandatoryInformationException(string message) : base(message)
        {
        }
    }
}
