using Back_Market_Vinci.Config;
using Back_Market_Vinci.Domaine;
using Back_Market_Vinci.Domaine.Exceptions;
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
                new Product(p.Id, p.Name, p.State.Value, p.Description, p.IsValidated.Value, p.ReasonNotValidated, p.SellerMail,
                p.SellerId, p.Adress, p.SentType.Value, p.Price.Value,p.Type.Value,p.Medias, p.BlobMedias, p.Video, p.BlobVideo)).ToList<IProductDTO>();
        }

        public IProductDTO GetProductById(string id)
        {
            IProductDTO productFound = null;
            try
            {
                productFound = _productsTable.AsQueryable()
                    .Select(p => new Product(p.Id, p.Name, p.State.Value, p.Description, p.IsValidated.Value, p.ReasonNotValidated, p.SellerMail,
                    p.SellerId, p.Adress, p.SentType.Value, p.Price.Value, p.Type, p.Medias, p.BlobMedias, p.Video, p.BlobVideo))
                    .Where(p => p.Id.Equals(id)).Single<Product>();
            }
            catch(ArgumentNullException)
            {
                throw new ProductNotFoundException("Le produit avec l'id " + id + " n'a pas été trouvé");
            }
            catch(InvalidOperationException)
            {
                throw new ProductNotFoundException("Plusieurs produits ont été trouvés avec le même id ou le produit n'a pas été trouvé");
            }
            
            return productFound;
        }

        public List<IProductDTO> GetProductBySeller(string idSeller) {
            List<IProductDTO> productsFound = null;
            try
            {
                productsFound = _productsTable.AsQueryable()
                    .Select(p => new Product(p.Id, p.Name, p.State.Value, p.Description, p.IsValidated.Value, p.ReasonNotValidated, p.SellerMail,
                    p.SellerId, p.Adress, p.SentType.Value, p.Price.Value, p.Type, p.Medias, p.BlobMedias, p.Video, p.BlobVideo))
                    .Where(p => p.SellerId.Equals(idSeller)).ToList<IProductDTO>();
            }
            catch (ArgumentNullException)
            {
                throw new ProductNotFoundException("L'utilisateur avec l'id " + idSeller + " n'a pas été trouvé");
            }
            catch (InvalidOperationException)
            {
                throw new ProductNotFoundException("Plusieurs utilisateurs ont été trouvés avec le même id ou l'utilisateur n'a pas été trouvé");
            }

            return productsFound;
        }

        public IProductDTO UpdateProductById(string id, IProductDTO productIn)
        {
            IProductDTO productModified = _productsTable.FindOneAndReplace<Product>(p => p.Id.Equals(id), (Product)productIn);
            if (productModified == null) throw new ProductNotFoundException("Le produit avec l'id " + id + " n'a pas été trouvé");
            return productIn;
        }

        public void DeleteProductById(string id)
        {
            IProductDTO productDeleted = _productsTable.FindOneAndDelete<Product>(p => p.Id.Equals(id));
            if (productDeleted == null) throw new ProductNotFoundException("Le produit avec l'id " + id + " n'a pas été trouvé");
        }

        public IProductDTO CreateProduct(Product productToCreate)
        {
            _productsTable.InsertOne(productToCreate);
            return productToCreate;
        }

        public List<IProductDTO> GetProductsNotValidated()
        {
            return _productsTable.AsQueryable<Product>()
                .Select(p => new Product(p.Id, p.Name, p.State.Value, p.Description, p.IsValidated.Value,
                p.ReasonNotValidated, p.SellerMail, p.SellerId,
                p.Adress, p.SentType.Value, p.Price.Value, p.Type, p.Medias, p.BlobMedias, p.Video, p.BlobVideo))
                .Where(p => (!p.IsValidated.Value || p.IsValidated == null) && p.ReasonNotValidated == null)
                .ToList<IProductDTO>();
        }

        public IProductDTO UpdateValidationOfProductById(string id, IProductDTO productIn)
        {
            
            IProductDTO productModified = _productsTable.FindOneAndReplace<Product>(p => p.Id.Equals(id), (Product)productIn);
            if (productModified == null) throw new ProductNotFoundException("Le produit avec l'id " + id + " n'a pas été trouvé");
            return productIn;
        }
    }
}
