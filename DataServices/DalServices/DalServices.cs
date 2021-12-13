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

        public CloudStorageAccount GetcloudStorageAccount
        {
            get{ 
                return CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=blobuploadimage;AccountKey=BxpjEgWtc9bWa2fiu0J2Cuu0CeNoYH+ft4xpSvSD+2DCblvd5+atcoXYswrERxq9juWoQpMtKMIbOnZb4QXClA==;EndpointSuffix=core.windows.net");
            }
        
        }

        public CloudBlobContainer GetcloudBlobContainer
        {
            get {
                var cloudBlobClient = GetcloudStorageAccount.CreateCloudBlobClient();
                return cloudBlobClient.GetContainerReference("imagecontainer");
            }
        
        }
             
           
        
    }
}
