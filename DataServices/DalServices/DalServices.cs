using Back_Market_Vinci.Domaine;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;

using MongoDB.Driver;
    

namespace Back_Market_Vinci.DataServices
{
    public class DalServices : IDalServices
    {
        public IConfiguration Configuration { get; }

        private IMongoDatabase Database {
            get {
                string connectionString = Configuration["ConnectionStringProd"];
                if (connectionString == null)
                    connectionString = Configuration["ConnectionStringDev"];
                var client = new MongoClient(connectionString);
                return client.GetDatabase(Configuration["DatabaseProperties:DatabaseName"]);
            }
        }

        public DalServices(IConfiguration conf)
        {
            Configuration = conf;
        }

        public IMongoCollection<User> UsersCollection
        {
            get
            {
                return Database.GetCollection<User>(Configuration["DatabaseProperties:UsersCollectionName"]);
            }
        }

        public IMongoCollection<Product> ProductsCollection
        {
            get
            {
                return Database.GetCollection<Product>(Configuration["DatabaseProperties:ProductsCollectionName"]);
            }
        }

        public IMongoCollection<Ratings> RatingsCollection
        {
            get {
                return Database.GetCollection<Ratings>(Configuration["DatabaseProperties:RatingsCollectionName"]);
            }
        }

        public IMongoCollection<Badges> BadgesCollection
        {
            get
            {
                return Database.GetCollection<Badges>(Configuration["DatabaseProperties:BadgesCollectionName"]);
            }
        }
    }
}
