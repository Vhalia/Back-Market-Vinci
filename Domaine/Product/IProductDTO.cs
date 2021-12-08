using System;
using Back_Market_Vinci.Domaine.Other;

namespace Back_Market_Vinci.Domaine.Product
{
    public interface IProductDTO
    {
        public string Id { get; set; }
        public String Name { get; set; }

        public States State { get; set; }

        public String Description { get; set; }

        public Boolean IsValidated { get; set; }

        public String ReasonNotValidated { get; set; }

        public User Seller { get; set; }

        public String Adress { get; set; }

        public SentTypes SentType { get; set; }



    }
}
