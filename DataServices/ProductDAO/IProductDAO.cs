using Back_Market_Vinci.Domaine.Product;
using System.Collections.Generic;

namespace Back_Market_Vinci.Uc
{
    public interface IProductDAO
    {
        List<IProductDTO> GetProducts();
        IProductDTO UpdateProductById(string id, IProductDTO productToBeUpdated);
    }
}