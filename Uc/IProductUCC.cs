using Back_Market_Vinci.Domaine.Product;
using System.Collections.Generic;

namespace Back_Market_Vinci.Api
{
    public interface IProductUCC
    {
        List<IProductDTO> GetProducts();
        IProductDTO UpdateProductbyId(string id, IProductDTO productToUpdate);
    }
}