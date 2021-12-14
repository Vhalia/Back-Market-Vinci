using Back_Market_Vinci.Api;
using Back_Market_Vinci.Config;
using Back_Market_Vinci.DataServices;
using Back_Market_Vinci.Domaine;
using Back_Market_Vinci.Domaine.Exceptions;
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
            productToCreate.BlobMedias = new List<string>();
            if (productToCreate.SellerId == null || productToCreate.Adress == null || productToCreate.Description == null
                || productToCreate.Name == null || productToCreate.SentType == null || productToCreate.Type == null)
                throw new MissingMandatoryInformationException("Il manque des informations obligatoires pour créer un produit");
            if (productToCreate.SentType != SentTypes.AVendre
                && (productToCreate.Price != null || productToCreate.Price != 0))
                throw new ArgumentException("Un produit à donner ou à échanger ne peut pas avoir de prix");
            if (productToCreate.SentType == SentTypes.AVendre && (productToCreate.Price == null || productToCreate.Price == 0))
                throw new MissingMandatoryInformationException("Un produit en vente doit avoir un prix supérieur à 0");
            if (!Product.AddressesAvailable.Contains(productToCreate.Adress))
                throw new ArgumentException("L'adresse du produit n'est pas correcte");

            IUserDTO user = _userDAO.GetUserById(productToCreate.SellerId);
            if (user.IsBanned.Value) throw new UnauthorizedException("L'utilisateur est banni");
            foreach (UploadContentRequest m in productToCreate.Medias)
            {
                if (m.Content == null)
                {
                    throw new ArgumentNullException("Il manque le contenu de l'image");
                }
                if (m.FileName == null)
                {
                    throw new ArgumentNullException("Il manque le nom du fichier");
                }
                else
                {
                    m.Content = m.Content.Substring(m.Content.IndexOf(",") + 1);
                    _blobServices.UploadContentBlobAsync(m.Content, m.FileName, "produitsimages");
                    productToCreate.BlobMedias.Add("https://blobuploadimage.blob.core.windows.net/produitsimages/" + m.FileName);
                }
            }
            productToCreate.Medias = null;
            productToCreate.SellerMail = null;
            productToCreate.ReasonNotValidated = null;
            productToCreate.IsValidated = false;
            IProductDTO productCreated = _productDAO.CreateProduct((Product)productToCreate);
            productCreated.SellerMail = user.Mail;
            return productCreated;
        }

        public void DeleteProductById(string id)
        {
            _productDAO.DeleteProductById(id);
        }

        public IProductDTO GetProductById(string id)
        {
            IProductDTO productFromDb = _productDAO.GetProductById(id);
            IUserDTO user = _userDAO.GetUserById(productFromDb.SellerId);
            productFromDb.SellerMail = user.Mail;
            return productFromDb;
        }

        public List<IProductDTO> GetProducts()
        {
            List<IProductDTO> productsDTO = _productDAO.GetProducts();
            for (var i = 0; i < productsDTO.Count; i++)
            {
                IUserDTO user = _userDAO.GetUserById(productsDTO[i].SellerId);
                productsDTO[i].SellerMail = user.Mail;
                if (user.IsBanned.Value)
                {
                    productsDTO.Remove(productsDTO[i]);
                    i--;
                }
            }
            return productsDTO;
        }

        public List<IProductDTO> GetProductsNotValidated()
        {
            List<IProductDTO> productsNotValidatedDb = _productDAO.GetProductsNotValidated();
            foreach (IProductDTO product in productsNotValidatedDb)
            {
                IUserDTO user = _userDAO.GetUserById(product.SellerId);
                product.SellerMail = user.Mail;
            }
            return productsNotValidatedDb;
        }

        public IProductDTO UpdateProductbyId(string id, IProductDTO productIn)
        {
            if (productIn.Adress != null && !Product.AddressesAvailable.Contains(productIn.Adress))
                throw new ArgumentException("L'adresse n'est pas correcte");
            productIn.State = null;
            productIn.SellerMail = null;
            productIn.ReasonNotValidated = null;
            productIn.IsValidated = null;
            IProductDTO productDb = _productDAO.GetProductById(id);
            IProductDTO productToBeUpdated = CheckNullFields<IProductDTO>.CheckNull(productIn, productDb);

            if (productToBeUpdated.SentType != SentTypes.AVendre
                && (productToBeUpdated.Price != null || productToBeUpdated.Price != 0))
                throw new ArgumentException("Un produit à donner ou à échanger ne peut pas avoir de prix");
            if (productToBeUpdated.SentType == SentTypes.AVendre && (productToBeUpdated.Price == null || productToBeUpdated.Price == 0))
                throw new MissingMandatoryInformationException("Un produit en vente doit avoir un prix supérieur à 0");

            IProductDTO productUpdated = _productDAO.UpdateProductById(id, productToBeUpdated);
            IUserDTO user = _userDAO.GetUserById(productUpdated.SellerId);
            productUpdated.SellerMail = user.Mail;
            return productUpdated;
        }

        public IProductDTO UpdateValidationOfProductById(string id, IProductDTO productIn)
        {
            IProductDTO productDb = _productDAO.GetProductById(id);
            IProductDTO productToBeUpdated = CheckNullFields<IProductDTO>.CheckNull(productIn, productDb);
            if (productIn.IsValidated.Value) productToBeUpdated.ReasonNotValidated = null;
            IProductDTO productUpdated = _productDAO.UpdateValidationOfProductById(id, productToBeUpdated);
            IUserDTO user = _userDAO.GetUserById(productUpdated.SellerId);
            productUpdated.SellerMail = user.Mail;
            return productUpdated;
        }

    }
}
