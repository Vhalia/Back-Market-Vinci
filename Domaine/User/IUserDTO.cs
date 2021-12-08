using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine
{
    public interface IUserDTO
    {

        public string Id { get; set; }
        public String Name { get; set; }

        public String Surname { get; set; }

        public String Mail { get; set; }

        public int Like { get; set; }

        public int Dislike { get; set; }

        public String Campus { get; set; }

        public String Password { get; set; }

    }
}
