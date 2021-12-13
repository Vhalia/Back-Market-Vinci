using Back_Market_Vinci.Domaine;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Api
{
    [ApiController]
    [Route("[controller]")]
    public class ProductRessource : ControllerBase
    {
        private IProductUCC _productUCC;

        public ProductRessource(IProductUCC _productUCC)
        {
            this._productUCC = _productUCC;
        }


        [HttpGet]
        [Route("/products")]
        public List<IProductDTO> GetProducts()
        {
            return _productUCC.GetProducts();
        }

        [HttpPatch]
        [Route("/products/{id}")]
        public IProductDTO UpdateProductById(string id, Product productToBeUpdated)
        {
            return _productUCC.UpdateProductbyId(id, productToBeUpdated);
        }
    }
}
