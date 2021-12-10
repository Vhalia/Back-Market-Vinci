using Back_Market_Vinci.Domaine;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.DataServices
{

    public class RatingsDAO : IRatingsDAO
    {
        private IDalServices _dalServices;
        private IMongoCollection<Ratings> _ratingsTable;

        public RatingsDAO(IDalServices dalServices)
        {
            this._dalServices = dalServices;
            this._ratingsTable = _dalServices.RatingsCollection;
        }
        public void AddRatings(IRatingsDTO ratings)
        {
            _ratingsTable.InsertOne((Ratings) ratings);
        }

        public void UpdateRatings(IRatingsDTO ratings) {

            _ratingsTable.ReplaceOne<Ratings>(r => r.Id.Equals(ratings.Id), (Ratings)ratings);

            
        }

        public void DeleteRatings(string id) {
            _ratingsTable.DeleteOne(r => r.Id.Equals(id));
        }

        public IRatingsDTO GetRatingsById(string id) {
            IRatingsDTO ratings = _ratingsTable.AsQueryable().FirstOrDefault(r => r.Id.Equals(id));
            return ratings;
        }
    }
}
