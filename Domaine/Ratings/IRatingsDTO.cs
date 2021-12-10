using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine
{
    public interface IRatingsDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdRater { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]

        public string IdRated { get; set; }

        public int Like { get; set; }
    }
}
