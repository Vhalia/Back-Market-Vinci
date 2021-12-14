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

        public ProductUCC(IProductDAO productDAO, IUserDAO userDAO)
        {
           this._productDAO = productDAO;
           this._userDAO = userDAO;

        }

        public IProductDTO CreateProduct(IProductDTO productToCreate)
        {
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
            productToCreate.ReasonNotValidated = null;
            productToCreate.IsValidated = false;
            IProductDTO productCreated = _productDAO.CreateProduct((Product)productToCreate);
            AddSellerToProduct(productCreated);
            return productCreated;
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
            if (productIn.Adress != null && !Product.AddressesAvailable.Contains(productIn.Adress))
                throw new ArgumentException("L'adresse n'est pas correcte");
            productIn.State = null;
            productIn.Seller = null;
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
            AddSellerToProduct(productUpdated);
            return productUpdated;
        }

        public IProductDTO UpdateValidationOfProductById(string id, IProductDTO productIn)
        {
            IProductDTO productDb = _productDAO.GetProductById(id);
            IProductDTO productToBeUpdated = CheckNullFields<IProductDTO>.CheckNull(productIn, productDb);
            if (productIn.IsValidated.Value) productToBeUpdated.ReasonNotValidated = null;
            IProductDTO productUpdated = _productDAO.UpdateValidationOfProductById(id, productToBeUpdated);
            AddSellerToProduct(productUpdated);
            if (productUpdated.Seller.IsAdmin == null || !productUpdated.Seller.IsAdmin.Value)
                throw new UnauthorizedException("Vous n'avez pas le droit de modifier la validité d'un produit");
            return productUpdated;
        }

        private void AddSellerToProduct(IProductDTO product)
        {
            IUserDTO user = _userDAO.GetUserById(product.SellerId);
            if (user == null) throw new UserNotFoundException("L'utilisateur avec l'id " + product.SellerId + " n'a pas été trouvé");
            product.Seller = (User)user;
        }
    }
}
