using Back_Market_Vinci.Domaine;
using Back_Market_Vinci.Domaine.Other;
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

        [HttpGet]
        [Route("/products/{id}")]
        public IProductDTO GetProductById(string id)
        {
            return _productUCC.GetProductById(id);
        }

        [HttpDelete]
        [Route("/products/{id}")]
        public void DeleteProductById(string id)
        {
            _productUCC.DeleteProductById(id);
        }

        [HttpPost]
        [Route("/products")]
        public IProductDTO CreateProduct(Product productToCreate)
        {
            return _productUCC.CreateProduct(productToCreate);
        }

        [HttpGet]
        [Route("/products/notValidated")]
        public List<IProductDTO> GetProductsNotValidated()
        {
            return _productUCC.GetProductsNotValidated();
        }

        [HttpPatch]
        [Route("/products/{id}/validate")]
        public IProductDTO UpdateValidationOfProductById(string id, Product productIn)
        {
            return _productUCC.UpdateValidationOfProductById(id, productIn);
        }
    }
}
