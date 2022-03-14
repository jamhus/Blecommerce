﻿namespace Blecommerce.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProductsAsync();
        Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl);
        Task<ServiceResponse<Product>> GetProductAsync(int id);
        Task<ServiceResponse<ProductSearchDto>> SearchProducts(string searchText, int page);
        Task<ServiceResponse<List<Product>>> GetFeaturedProducts();

    }
}
