namespace Blecommerce.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<ProductListDto>> GetPagedProducts(ProductParams productParams, bool admin);
        Task<ServiceResponse<Product>> GetProductAsync(int id);
        Task<ServiceResponse<Product>> CreateProduct(Product product);
        Task<ServiceResponse<Product>> UpdateProduct(Product product);
        Task<ServiceResponse<bool>> DeleteProduct(int productId);
    }
}
