using Back_Market_Vinci.Config;
using Back_Market_Vinci.Domaine;
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
                new Product(p.Id, p.Name, p.State, p.Description, p.IsValidated.Value, p.ReasonNotValidated, p.Seller,
                p.SellerId, p.Adress, p.SentType, p.Price.Value,p.Medias, p.BlobMedias)).ToList<IProductDTO>();
        }

        public IProductDTO GetProductById(string id)
        {
            return _productsTable.AsQueryable()
                .Select(p => new Product(p.Id, p.Name, p.State, p.Description, p.IsValidated.Value, p.ReasonNotValidated, p.Seller,
                p.SellerId, p.Adress, p.SentType, p.Price.Value, p.Medias, p.BlobMedias))
                .Where(p => p.Id.Equals(id)).Single<Product>();
        }

        public IProductDTO UpdateProductById(string id, IProductDTO productIn)
        {
            _productsTable.ReplaceOne<Product>(p => p.Id.Equals(id), (Product)productIn);

            return GetProductById(productIn.Id);
        }

        public void DeleteProductById(string id)
        {
            _productsTable.DeleteOne<Product>(p => p.Id.Equals(id));
        }

        public IProductDTO CreateProduct(Product productToCreate)
        {
            _productsTable.InsertOne(productToCreate);
            return productToCreate;
        }

        public List<IProductDTO> GetProductsNotValidated()
        {
            return _productsTable.AsQueryable<Product>()
                .Select(p => new Product(p.Id, p.Name, p.State, p.Description, p.IsValidated.Value,
                p.ReasonNotValidated, p.Seller, p.SellerId,
                p.Adress, p.SentType, p.Price.Value, p.Medias, p.BlobMedias))
                .Where(p => (!p.IsValidated.Value || p.IsValidated == null) && p.ReasonNotValidated == null)
                .ToList<IProductDTO>();
        }

        public IProductDTO UpdateValidationOfProductById(string id, IProductDTO productIn)
        {
            _productsTable.ReplaceOne<Product>(p => p.Id.Equals(id), (Product)productIn);
            return productIn;
        }
    }
}
