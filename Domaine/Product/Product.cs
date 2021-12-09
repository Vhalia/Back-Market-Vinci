using Back_Market_Vinci.Domaine.Other;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Back_Market_Vinci.Domaine.Product
{
    public class Product : IProduct, IProductDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get ; set ; }
        public States State { get ; set ; }
        public string Description { get ; set ; } 
        public bool? IsValidated { get ; set ; }
        public string ReasonNotValidated { get ; set ; }
        public User Seller { get ; set ; }
        public string Adress { get ; set ; }
        public SentTypes? SentType { get ; set ; }

        public Product(string id, string name, States state, string description, bool isValidated, string reasonNotValidated, User seller, string adress, SentTypes sentType)
        {
            Id = id;
            Name = name;
            State = state;
            Description = description;
            IsValidated = isValidated;
            ReasonNotValidated = reasonNotValidated;
            Seller = seller;
            Adress = adress;
            SentType = sentType;
        }
    }
}
