using Back_Market_Vinci.Api;
using Back_Market_Vinci.Config;
using Back_Market_Vinci.DataServices;
using Back_Market_Vinci.Domaine;
using Back_Market_Vinci.Domaine.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Uc
{
    public class ProductUCC : IProductUCC
    {

        private IProductDAO _productDAO;
        private IUserDAO _userDAO;
        private IBlobService _blobServices;

        public ProductUCC(IProductDAO productDAO, IUserDAO userDAO, IBlobService blobServices)
        {
           this._productDAO = productDAO;
           this._userDAO = userDAO;
            this._blobServices = blobServices;

        }

        public IProductDTO CreateProduct(IProductDTO productToCreate)
        {
            foreach(UploadContentRequest m in productToCreate.Medias){
                if (m.Content == null)
                {
                    throw new ArgumentNullException("Il manque le contenu de l'image");
                }
                if (m.FileName == null)
                {
                    throw new ArgumentNullException("Il manque le nom du fichier");
                }
                else {
                    m.Content = m.Content.Substring(m.Content.IndexOf(",") + 1);
                    _blobServices.UploadContentBlobAsync(m.Content, m.FileName, "produitsimages");
                    productToCreate.BlobMedias.Add("https://blobuploadimage.blob.core.windows.net/imagecontainer/" + m.FileName);
                }
            }
            productToCreate.Medias = null;
            AddSellerToProduct(productToCreate);
            return _productDAO.CreateProduct((Product)productToCreate);
        }

        public void DeleteProductById(string id)
        {
            _productDAO.DeleteProductById(id);
        }

        public IProductDTO GetProductById(string id)
        {
            IProductDTO productFromDb = _productDAO.GetProductById(id);
            AddSellerToProduct(productFromDb);
            return productFromDb;
        }

        public List<IProductDTO> GetProducts()
        {
            List<IProductDTO> productsDTO = _productDAO.GetProducts();
            for (var i = 0; i < productsDTO.Count; i++)
            {
                AddSellerToProduct(productsDTO[i]);
                if (productsDTO[i].Seller.IsBanned.Value) productsDTO.Remove(productsDTO[i]);
            }
            return productsDTO;
        }

        public List<IProductDTO> GetProductsNotValidated()
        {
            List<IProductDTO> productsNotValidatedDb = _productDAO.GetProductsNotValidated();
            foreach (IProductDTO product in productsNotValidatedDb)
            {
                AddSellerToProduct(product);
            }
            return productsNotValidatedDb;
        }

        public IProductDTO UpdateProductbyId(string id, IProductDTO productIn)
        {
            IProductDTO productDb = _productDAO.GetProductById(id);
            IProductDTO productToBeUpdated = CheckNullFields<IProductDTO>.CheckNull(productIn, productDb);
            IProductDTO productUpdated = _productDAO.UpdateProductById(id, productToBeUpdated);
            AddSellerToProduct(productUpdated);
            return productUpdated;
        }

        public IProductDTO UpdateValidationOfProductById(string id, IProductDTO productIn)
        {
            IProductDTO productDb = _productDAO.GetProductById(id);
            IProductDTO productToBeUpdated = CheckNullFields<IProductDTO>.CheckNull(productIn, productDb);
            if (productIn.IsValidated.Value) productToBeUpdated.ReasonNotValidated = null;
            return _productDAO.UpdateValidationOfProductById(id, productToBeUpdated);
        }

        private void AddSellerToProduct(IProductDTO product)
        {
            IUserDTO user = _userDAO.GetUserById(product.SellerId);
            product.Seller = (User)user;
        }
    }
}
