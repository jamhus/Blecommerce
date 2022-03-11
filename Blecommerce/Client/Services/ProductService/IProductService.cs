
namespace Blecommerce.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> products { get; set; }
        string Message { get; set; }    
        Task GetProducts(string? categoryUrl = null);
        Task SearchProducts(string searchtext);
        Task<ServiceResponse<Product>> GetProduct(int id);
    }
}
