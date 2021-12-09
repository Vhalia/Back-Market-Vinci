using Back_Market_Vinci.Domaine.Product;
using Back_Market_Vinci.Uc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.DataServices.ProductDAO
{
    public class ProductDAO : IProductDAO
    {
        private IDalServices _dalServices;
        private IMongoCollection<Product> _productsTable;

        public ProductDAO(IDalServices dalServices)
        {
            this._dalServices = dalServices;
            this._productsTable = _dalServices.ProductsCollection;
        }

        public List<IProductDTO> GetProducts()
        {
            return _productsTable.AsQueryable().Select(p =>
                new Product(p.Id, p.Name, p.State, p.Description, p.IsValidated.Value, p.ReasonNotValidated,
                p.Seller, p.Adress, p.SentType.Value)).ToList<IProductDTO>();
        }

        public IProductDTO GetProductById(string id)
        {
            return _productsTable.AsQueryable().Single(u => u.Id == id);
        }

        public IProductDTO UpdateProductById(string id, IProductDTO productToBeUpdated)
        {
            //TODO checkNull

            var result = _productsTable.ReplaceOne<Product>(p => p.Id.Equals(id), (Product) productToBeUpdated);

            return GetProductById(productToBeUpdated.Id);
        }
    }
}
