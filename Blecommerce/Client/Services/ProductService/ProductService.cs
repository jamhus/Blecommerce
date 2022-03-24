﻿
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
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchString { get; set; } = String.Empty;   

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

        public async Task GetAdminProducts()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/admin");
            AdminProducts = result.Data;
            CurrentPage = 1;
            PageCount = 0;
            if (AdminProducts.Count == 0)
                Message = "No products found.";
        }

        public async Task<ServiceResponse<Product>> GetProduct(int id)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{id}");
            if (result != null && result.Data != null)
            return result;
            return new ServiceResponse<Product>();
        }

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result =
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>
                (categoryUrl == null ? "api/product/featured" : $"api/product/category/{categoryUrl}");
            if(result != null && result.Data != null)
            products = result.Data;

            ProductsChanged.Invoke();
        }


        public async Task SearchProducts(string searchtext, int page)
        {
            LastSearchString = searchtext;
            var result = await _http.GetFromJsonAsync<ServiceResponse<ProductSearchDto>>($"api/product/search/{searchtext}/{page}");
            if (result != null && result.Data != null)
            {
                products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            if (products.Count == 0)
            {
                Message = "no products found";
            }

            ProductsChanged.Invoke();
        }
    }
}
