using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Back_Market_Vinci.Config;
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

        public List<String> FavTypes { get; set; }

        public List<Product> Selled { get; set; }

        public List<Product> Buyed { get; set; }

        public List<Product> FavProduct { get; set; }

        public List<Badges> Badges { get; set; }

        public User(string id, string name, string surname, string mail, string campus, string password, Boolean? IsBanned, Boolean? IsAdmin, List<Ratings> Ratings) {
            this.Id = id;
            this.Name = name;
            this.Surname = surname;
            this.Mail = mail;
            this.Campus = campus;
            this.Password = password;
            this.IsBanned = IsBanned;
            this.IsAdmin = IsAdmin;
            this.Ratings = Ratings;
            
        }
    }
}
