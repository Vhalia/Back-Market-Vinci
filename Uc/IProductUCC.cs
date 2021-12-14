using Back_Market_Vinci.Domaine;
using Back_Market_Vinci.Domaine.Other;
using System;
using System.Collections.Generic;

namespace Back_Market_Vinci.Api
{
    public interface IProductUCC
    {
        List<IProductDTO> GetProducts();
        IProductDTO UpdateProductbyId(string id, IProductDTO productToUpdate);
        IProductDTO GetProductById(string id);
        void DeleteProductById(string id);
        IProductDTO CreateProduct(IProductDTO productToCreate);
        List<IProductDTO> GetProductsNotValidated();
        IProductDTO UpdateValidationOfProductById(string id, IProductDTO productIn);

        IProductDTO SellProduct(string idProduct, string idClient);
    }
}