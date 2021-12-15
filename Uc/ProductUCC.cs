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
                && (productToCreate.Price != null && productToCreate.Price != 0))
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
            if (productToCreate.BlobMedias.Count == 0) {
                productToCreate.BlobMedias.Add("https://blobuploadimage.blob.core.windows.net/produitsimages/defaultproduct.png");
            }
            
            IProductDTO productCreated = _productDAO.CreateProduct((Product)productToCreate);
            productCreated.SellerMail = user.Mail;
            List<IProductDTO> toSell = _productDAO.GetProductBySeller(productToCreate.SellerId);
            if (toSell.Count == 1)
            {
                user.Badges.ElementAt(9).IsUnlocked = true;
            }
            else if (toSell.Count == 3)
            {
                user.Badges.ElementAt(10).IsUnlocked = true;
            }
            else if (toSell.Count == 5)
            {
                user.Badges.ElementAt(11).IsUnlocked = true;
                if (user.Badges.ElementAt(3).IsUnlocked && user.Badges.ElementAt(6).IsUnlocked && user.Badges.ElementAt(7).IsUnlocked && user.Badges.ElementAt(11).IsUnlocked)
                {
                    user.Badges.ElementAt(8).IsUnlocked = true;
                }
            }
            _userDAO.UpdateUser(user);
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
                && (productToBeUpdated.Price != null && productToBeUpdated.Price != 0))
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
            if (productIn.IsValidated.Value) {
                productToBeUpdated.ReasonNotValidated = null;
                productToBeUpdated.State = States.EnLigne;
            }else
            {
                productToBeUpdated.State = States.Refuse;
            }
            IProductDTO productUpdated = _productDAO.UpdateValidationOfProductById(id, productToBeUpdated);
            IUserDTO user = _userDAO.GetUserById(productUpdated.SellerId);
            productUpdated.SellerMail = user.Mail;
            return productUpdated;
        }

        public IProductDTO SellProduct(string idProduct, string idClient) {
            IProductDTO productDB = _productDAO.GetProductById(idProduct);
            IUserDTO clientDB = _userDAO.GetUserById(idClient);
            IUserDTO sellerDB = _userDAO.GetUserById(productDB.SellerId);
            sellerDB.Sold.Add(idProduct);
            if (productDB.State != States.EnLigne) {
                throw new WrongStateException("Le produit doit etre en ligne pour pouvoir etre vendus");
            }

            var numberProductSold = sellerDB.Sold.Count;
            if (numberProductSold == 1)
            {
                sellerDB.Badges.ElementAt(1).IsUnlocked = true;
            }
            else if (numberProductSold == 3)
            {
                sellerDB.Badges.ElementAt(2).IsUnlocked = true;
            }
            else if (numberProductSold == 5) {
                sellerDB.Badges.ElementAt(3).IsUnlocked = true;
                if (sellerDB.Badges.ElementAt(3).IsUnlocked && sellerDB.Badges.ElementAt(6).IsUnlocked && sellerDB.Badges.ElementAt(7).IsUnlocked && sellerDB.Badges.ElementAt(11).IsUnlocked) {
                    sellerDB.Badges.ElementAt(8).IsUnlocked = true;
                }
            }

            clientDB.Bought.Add(idProduct);
            var numberBought = clientDB.Bought.Count;
            if (numberBought == 1)
            {
                clientDB.Badges.ElementAt(4).IsUnlocked = true;
            }
            else if (numberBought == 3)
            {
                clientDB.Badges.ElementAt(5).IsUnlocked = true;
                var rat = 0;
                foreach (string idP in clientDB.Bought) {
                    IProductDTO p = _productDAO.GetProductById(idP);
                    if (p.SentType == SentTypes.ADonner) {
                        rat++;
                    }
                }
                if (rat == 3) {
                    clientDB.Badges.ElementAt(7).IsUnlocked = true;
                }
            }
            else if (numberBought == 5)
            {
                clientDB.Badges.ElementAt(6).IsUnlocked = true;
                if (sellerDB.Badges.ElementAt(3).IsUnlocked && sellerDB.Badges.ElementAt(6).IsUnlocked && sellerDB.Badges.ElementAt(7).IsUnlocked && sellerDB.Badges.ElementAt(11).IsUnlocked)
                {
                    sellerDB.Badges.ElementAt(8).IsUnlocked = true;
                }
            }
            
            productDB.State = States.Envoye;
            _userDAO.UpdateUser(clientDB);
            _userDAO.UpdateUser(sellerDB);
            _productDAO.UpdateProductById(idProduct, productDB);
            
            return productDB;
        }

    }
}
