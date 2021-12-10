using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Domaine
{
    public class Ratings : IRatings, IRatingsDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdRater { get ; set ; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdRated { get ; set ; }
        public int Like { get ; set; }

        public Ratings(string IdRater, string IdRated, int Like) {
            this.IdRated = IdRated;
            this.IdRater = IdRater;
            this.Like = Like;
        }
    }
}
