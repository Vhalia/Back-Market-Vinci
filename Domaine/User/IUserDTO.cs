﻿using Back_Market_Vinci.Config;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine
{
    public interface IUserDTO
    {

        public string Id { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Mail { get; set; }

        [BsonSerializer(typeof(NullableIntAsIntSerializer))]
        public int? Like { get; set; }

        [BsonSerializer(typeof(NullableIntAsIntSerializer))]
        public int? Dislike { get; set; }

        public string Campus { get; set; }

        public string Password { get; set; }

        [BsonSerializer(typeof(NullableBooleanAsBooleanSerializer))]
        public Boolean? IsBanned { get; set; }

        [BsonSerializer(typeof(NullableBooleanAsBooleanSerializer))]
        public Boolean? IsAdmin { get; set; }
    }
}
