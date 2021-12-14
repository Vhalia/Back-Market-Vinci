using Back_Market_Vinci.Domaine;
using Microsoft.Azure.Storage;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.DataServices
{
    public interface IDalServices
    {

        IMongoCollection<User> UsersCollection { get; }
        IMongoCollection<Product> ProductsCollection { get; }
        IMongoCollection<Ratings> RatingsCollection { get; }
        IMongoCollection<Badges> BadgesCollection { get; }

         
    }
}
