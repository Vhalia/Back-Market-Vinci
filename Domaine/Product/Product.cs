﻿using Back_Market_Vinci.Config;
using Back_Market_Vinci.Domaine.Other;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Back_Market_Vinci.Domaine.Product
{
    public class Product : IProduct, IProductDTO, IProductDb
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get ; set ; }
        public States State { get ; set ; }
        public string Description { get ; set ; }

        [BsonSerializer(typeof(NullableBooleanAsBooleanSerializer))]
        public Boolean? IsValidated { get ; set ; }
        public string ReasonNotValidated { get ; set ; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string SellerId { get; set; }
        public User Seller { get ; set ; }
        public string Adress { get ; set ; }
        public SentTypes SentType { get ; set ; }

        [BsonSerializer(typeof(NullableIntAsIntSerializer))]
        public int? Price { get; set; }

        public Product(string id, string name, States state, string description, Boolean? isValidated, string reasonNotValidated, User seller, string sellerId, string adress, SentTypes sentType, int? price)
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
        }
    }
}
