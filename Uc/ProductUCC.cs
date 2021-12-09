using Back_Market_Vinci.Api;
using Back_Market_Vinci.Domaine.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Uc
{
    public class ProductUCC : IProductUCC
    {

        private IProductDAO _productDAO;

        public ProductUCC(IProductDAO productDAO)
        {
           this._productDAO = productDAO;
        }

        public List<IProductDTO> GetProducts()
        {
            return _productDAO.GetProducts();
        }

        public IProductDTO UpdateProductbyId(string id, IProductDTO productToBeUpdated)
        {
            return _productDAO.UpdateProductById(id, productToBeUpdated);
        }
    }
}
