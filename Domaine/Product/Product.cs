using Back_Market_Vinci.Config;
using Back_Market_Vinci.Domaine.Other;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using System;
using System.Text.Json.Serialization;

namespace Back_Market_Vinci.Domaine
{
    public class Product : IProduct, IProductDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get ; set ; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public States? State { get ; set ; }
        public string Description { get ; set ; }

        [BsonSerializer(typeof(NullableBooleanAsBooleanSerializer))]
        public Boolean? IsValidated { get ; set ; }
        public string ReasonNotValidated { get ; set ; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string SellerId { get; set; }
        public User Seller { get ; set ; }
        public string Adress { get ; set ; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public SentTypes? SentType { get ; set ; }

        [BsonSerializer(typeof(NullableIntAsIntSerializer))]
        public int? Price { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public Types? Type { get; set; }

        public Product(string id, string name, States? state, string description, Boolean? isValidated,
            string reasonNotValidated, User seller, string sellerId, string adress, SentTypes? sentType,
            int? price, Types? type)
        {
            Id = id;
            Name = name;
            State = state;
            Description = description;
            IsValidated = isValidated;
            ReasonNotValidated = reasonNotValidated;
            Seller = seller;
            SellerId = sellerId;
            Adress = adress;
            SentType = sentType;
            Price = price;
            Type = type;
        }
    }
}
