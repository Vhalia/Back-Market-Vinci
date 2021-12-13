using Back_Market_Vinci.Api;
using Back_Market_Vinci.Config;
using Back_Market_Vinci.DataServices;
using Back_Market_Vinci.Domaine;
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

        public List<IProductDTO> GetProducts()
        {
            List<IProductDTO> productsDTO = _productDAO.GetProducts();
            foreach (IProductDTO product in productsDTO)
            {
                IUserDTO user = _userDAO.GetUserById(product.SellerId);
                product.Seller = (User)user;
            }
            return productsDTO;
        }

        public IProductDTO UpdateProductbyId(string id, IProductDTO productIn)
        {
            IProductDTO productDb = _productDAO.GetProductById(id);
            IProductDTO productToBeUpdated = CheckNullFields<IProductDTO>.CheckNull(productIn, productDb);
            IProductDTO productUpdated = _productDAO.UpdateProductById(id, productToBeUpdated);
            productUpdated.Seller = (User) _userDAO.GetUserById(productUpdated.SellerId);
            return productUpdated;
        }
    }
}
