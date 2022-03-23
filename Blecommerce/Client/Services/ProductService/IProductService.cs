
namespace Blecommerce.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> products { get; set; }
        List<Product> AdminProducts { get; set; }
        string Message { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchString { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task GetAdminProducts();
        Task SearchProducts(string searchtext, int page);
        Task<ServiceResponse<Product>> GetProduct(int id);
    }
}
