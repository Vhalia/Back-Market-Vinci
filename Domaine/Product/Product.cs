using Back_Market_Vinci.Config;
using Back_Market_Vinci.Domaine.Other;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
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
        public string SellerMail { get ; set ; }
        public string Adress { get ; set ; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public SentTypes? SentType { get ; set ; }

        [BsonSerializer(typeof(NullableIntAsIntSerializer))]
        public int? Price { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public Types? Type { get; set; }

        public List<UploadContentRequest> Medias { get; set; }

        public List<string> BlobMedias { get; set; }

        public UploadContentRequest Video { get; set; }

        public string BlobVideo { get; set; }

        public static List<string> AddressesAvailable
        {
            get
            {
                List<string> addresses = new List<string>();
                addresses.Add("Place de l'Alma 3, 1200 Woluwe-Saint-Lambert");
                addresses.Add("Clos Chapelle-aux-Champs 43, 1200 Woluwe-Saint-Lambert");
                addresses.Add("Promenade de l'Alma 59, 1200 Woluwe-Saint-Lambert");
                addresses.Add("Place de l'Alma 2, 1200 Woluwe-Saint-Lambert");
                addresses.Add("Chaussée de Wavre 249, 1050 Ixelles");
                addresses.Add("Rue de Trèves 84, 1050 Ixelles");
                addresses.Add("Rue Limauge 14, 1050 Ixelles");
                addresses.Add("Rue d'Arlon 11, 1050 Ixelles");
                addresses.Add("Voie Cardijn 10, 1348 Ottignies-Louvain-la-Neuve");
                addresses.Add("Rue du Traité de Rome, 1348 Ottignies-Louvain-la-Neuve");
                addresses.Add("Rue de l'Union européenne 4, 1348 Ottignies-Louvain-la-Neuve");
                addresses.Add("Chemin de la Bardane 17, 1348 Ottignies-Louvain-la-Neuve");
                addresses.Add("Rue Paulin Ladeuze 14, 1348 Ottignies-Louvain-la-Neuve");
                return addresses;
            }
        }

        public Product(string id, string name, States? state, string description, Boolean? isValidated,
            string reasonNotValidated, string sellerMail, string sellerId, string adress, SentTypes? sentType,
            int? price, Types? type, List<UploadContentRequest> medias, List<string> blobMedias, UploadContentRequest video, string blobVideo)
        {
            Id = id;
            Name = name;
            State = state;
            Description = description;
            IsValidated = isValidated;
            ReasonNotValidated = reasonNotValidated;
            SellerMail = sellerMail;
            SellerId = sellerId;
            Adress = adress;
            SentType = sentType;
            Price = price;
            Medias = medias;
            BlobMedias = blobMedias;
            Type = type;
            Video = video;
            BlobVideo = blobVideo;
        }
    }
}
