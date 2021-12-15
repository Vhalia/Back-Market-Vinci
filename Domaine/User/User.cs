using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Back_Market_Vinci.Config;
using Back_Market_Vinci.Domaine.Other;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Back_Market_Vinci.Domaine
{
    public class User : IUser, IUserDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }
        public string Mail { get; set; }

        public string Campus { get; set; }
        public string Password { get; set; }
        [BsonSerializer(typeof(NullableBooleanAsBooleanSerializer))]
        public Boolean? IsBanned { get; set; }
        [BsonSerializer(typeof(NullableBooleanAsBooleanSerializer))]
        public Boolean? IsAdmin { get; set; }

        public List<Ratings> Ratings { get; set; }

        public string Image { get; set; }

        public List<Types> FavTypes { get; set; }
        [JsonIgnore]
        public List<string> Sold { get; set; }
        [JsonIgnore]
        public List<string> Bought { get; set; }
        [JsonIgnore]
        public List<string> FavProducts { get; set; }

        public List<Badges> Badges { get; set; }

        public static List<string> CampusAvailable { 
            get
            {
                List<string> campus = new List<string>();
                campus.Add("Ixelles");
                campus.Add("Woluwe-saint-lambert");
                campus.Add("Louvain-la-Neuve");
                return campus;
            }
        }

        public User(string id, string name, string surname,
            string mail, string campus, string password,
            Boolean? IsBanned, Boolean? IsAdmin, List<string> Sold,
            List<string> Bought, List<Types> FavTypes, List<string> FavProducts,
            List<Badges> Badges, string Image , List<Ratings> Ratings) {
            this.Id = id;
            this.Name = name;
            this.Surname = surname;
            this.Mail = mail;
            this.Campus = campus;
            this.Password = password;
            this.IsBanned = IsBanned;
            this.IsAdmin = IsAdmin;
            this.Badges = Badges;
            this.Sold = Sold;
            this.Bought = Bought;
            this.FavProducts = FavProducts;
            this.FavTypes = FavTypes;
            this.Image = Image;
            this.Ratings = Ratings;
            
        }
    }
}
