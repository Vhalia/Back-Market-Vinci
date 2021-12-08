using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.DataServices
{
    public class DalServices : IDalServices
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UsersCollectionName { get; set; } = null!;
        public IMongoDatabase GetDatabase()
        {
            var settings = "mongodb+srv://root:azerty@pfe-db.91mid.mongodb.net/PFE-DB?retryWrites=true&w=majority";
            var client = new MongoClient(settings);
            return client.GetDatabase("PFE-DB");
        }
    }
}
