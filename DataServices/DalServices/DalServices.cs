using Back_Market_Vinci.Domaine;
using Back_Market_Vinci.Domaine.Product;
using Microsoft.Extensions.Configuration;

using MongoDB.Driver;
    

namespace Back_Market_Vinci.DataServices
{
    public class DalServices : IDalServices
    {
        public IConfiguration Configuration { get; }

        private IMongoDatabase Database {
            get {
                var client = new MongoClient(Configuration["DatabaseProperties:ConnectionString"]);
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
    }
}
