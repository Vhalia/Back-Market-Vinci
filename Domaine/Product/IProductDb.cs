using Back_Market_Vinci.Config;
using Back_Market_Vinci.Domaine.Other;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine.Product
{
    public interface IProductDb
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public States State { get; set; }

        public string Description { get; set; }

        [BsonSerializer(typeof(NullableBooleanAsBooleanSerializer))]
        public Boolean? IsValidated { get; set; }

        public string ReasonNotValidated { get; set; }

        public string SellerId { get; set; }

        public string Adress { get; set; }

        public SentTypes SentType { get; set; }
    }
}
