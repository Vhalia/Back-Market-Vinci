﻿using System;
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
        public int Like { get; set; }
        public int Dislike { get; set; }
        public string Campus { get; set; }
        
        public string Password { get; set; }

        public User(String name, String surname) {
            this.Name = name;
            this.Surname = surname;
        }
    }
}