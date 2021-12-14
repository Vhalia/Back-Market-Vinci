using Back_Market_Vinci.Config;
using Back_Market_Vinci.Domaine.Other;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


namespace Back_Market_Vinci.Domaine
{
    public interface IUserDTO
    {

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

        public List<string> Sold { get; set; }

        public List<string> Bought { get; set; }

        public List<string> FavProducts { get; set; }
        public List<Badges> Badges { get; set; }
    }
}
