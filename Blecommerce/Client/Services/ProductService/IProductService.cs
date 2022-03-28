
namespace Blecommerce.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> products { get; set; }
        List<Product> AdminProducts { get; set; }
        List<string> CurrentCategories { get; set; }
        string Message { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        MetaData MetaData { get; set; }
        string LastSearchString { get; set; }
        Task GetAdminProducts(int page);
        Task<ServiceResponse<Product>> GetProduct(int id);
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(Product product);
        Task GetPagedProducts(int page);
        void Paginate(int page, bool admin);
        void Filter(Category category);
        void Search(bool admin);
    }
}
