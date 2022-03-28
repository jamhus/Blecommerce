
namespace Blecommerce.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }
        public List<Product> products { get; set; } = new List<Product>();
        public List<Product> AdminProducts { get; set; } = new List<Product>();
        public string Message { get; set; } = "Loading products...";
        public MetaData MetaData { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchString { get; set; } = String.Empty;
        public List<string> CurrentCategories { get; set; } = new List<string>();

        public event Action ProductsChanged = default!;

        public async Task<Product> CreateProduct(Product product)
        {
            var result = await _http.PostAsJsonAsync("api/product", product);
            var newProduct = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
            return newProduct;
        }

        public async Task DeleteProduct(Product product)
        {
            await _http.DeleteAsync($"api/product/{product.Id}");
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var result = await _http.PutAsJsonAsync($"api/product", product);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>();
            return content.Data;
        }

        public async Task GetAdminProducts(int page)
        {
            string categories = string.Join(",", CurrentCategories);

            var result = await _http.GetFromJsonAsync<ServiceResponse<ProductListDto>>(
                $"api/product/admin?SearchText={LastSearchString}&Categories={categories}&PageNumber={page}&PageSize=6");
            if (result != null && result.Data != null)
            {
                AdminProducts = result.Data.Products;
                MetaData = result.Data.MetaData;
            }

            if (products.Count == 0)
            {
                Message = "no products found";
            }
            ProductsChanged.Invoke();


        }

        public async Task<ServiceResponse<Product>> GetProduct(int id)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{id}");
            if (result != null && result.Data != null)
                return result;
            return new ServiceResponse<Product>();
        }

        public async Task GetPagedProducts(int page)
        {
            string categories = string.Join(",", CurrentCategories);

            var result = await _http.GetFromJsonAsync<ServiceResponse<ProductListDto>>(
                $"api/product/paged?SearchText={LastSearchString}&Categories={categories}&PageNumber={page}&PageSize=4");
            if (result != null && result.Data != null)
            {
                products = result.Data.Products;
                MetaData = result.Data.MetaData;
            }

            if (products.Count == 0)
            {
                Message = "no products found";
            }

            ProductsChanged.Invoke();
        }

        public async void Paginate(int page, bool admin)
        {
            if (admin)
            {
                await GetAdminProducts(page);
            }
            else
            {
                await GetPagedProducts(page);
            }
        }
        public async void Search(bool admin)
        {
            if (admin)
            {
                await GetAdminProducts(1);
            }
            else
            {
                await GetPagedProducts(1);
            }
        }

        public async void Filter(Category category)
        {
            if (CurrentCategories.Contains(category.Url))
            {
                CurrentCategories.Remove(category.Url);
            }
            else
            {
                CurrentCategories.Add(category.Url);
            }
            await GetPagedProducts(1);
        }
    }
}
