﻿namespace Blecommerce.Client.Services.ProductTypesService
{
    public interface IProductTypeService
    {
        event Action OnChange;
        List<ProductType> ProductTypes { get; set; }
        Task GetProductTypes();
        Task AddProductType(ProductType productType);
        Task UpdateProductType(ProductType productType);
        ProductType CreateNewProductType();
    }
}
