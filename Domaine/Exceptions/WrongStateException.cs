﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine.Exceptions
{
    public class WrongStateException : Exception
    {
        public WrongStateException() { }

        public WrongStateException(string message) : base(message)
        {
        }
    }
}
