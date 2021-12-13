﻿using Back_Market_Vinci.Domaine;
using System.Collections.Generic;

namespace Back_Market_Vinci.Uc
{
    public interface IProductDAO
    {
        List<IProductDTO> GetProducts();

        public IProductDTO GetProductById(string id);
        IProductDTO UpdateProductById(string id, IProductDTO productToBeUpdated);
    }
}