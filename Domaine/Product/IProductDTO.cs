using System;
using System.Collections.Generic;
using Back_Market_Vinci.Config;
using Back_Market_Vinci.Domaine.Other;
using MongoDB.Bson.Serialization.Attributes;

namespace Back_Market_Vinci.Domaine
{
    public interface IProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public States State { get; set; }

        public string Description { get; set; }

        [BsonSerializer(typeof(NullableBooleanAsBooleanSerializer))]
        public Boolean? IsValidated { get; set; }

        public string ReasonNotValidated { get; set; }

        public string SellerId { get; set; }

        public User Seller { get; set; }

        public string Adress { get; set; }

        public SentTypes SentType { get; set; }

        [BsonSerializer(typeof(NullableIntAsIntSerializer))]
        public int? Price { get; set; }

        public List<UploadContentRequest> Medias { get; set; }

        public List<string> BlobMedias { get; set; }

    }
}
