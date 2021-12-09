using System;
using Back_Market_Vinci.Domaine.Other;

namespace Back_Market_Vinci.Domaine.Product
{
    public interface IProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public States State { get; set; }

        public string Description { get; set; }

        public bool? IsValidated { get; set; }

        public string ReasonNotValidated { get; set; }

        public User Seller { get; set; }

        public string Adress { get; set; }

        public SentTypes? SentType { get; set; }



    }
}
