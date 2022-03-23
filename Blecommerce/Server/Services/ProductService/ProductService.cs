using Microsoft.EntityFrameworkCore;

namespace Blecommerce.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Product>>> GetAdminProducts()
        {
            var products = await _context.Products
             .Where(p => !p.Deleted)
             .Include(p => p.Variants.Where(v =>!v.Deleted))
             .ThenInclude(v=> v.ProductType)
             .ToListAsync();

            var response = new ServiceResponse<List<Product>>()
            {
                Data = products
            };
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            var products = await _context.Products
            .Where(p => p.Visible && !p.Deleted && p.Featured)
            .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
            .ToListAsync();

            var response = new ServiceResponse<List<Product>>()
            {
                Data = products
            };
            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int id)
        {
            var product = await _context.Products
                .Where(p => p.Visible && !p.Deleted)
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                .ThenInclude(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id);
            var response = new ServiceResponse<Product>();
            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry , this product doesn't exist";
            }
            else
            {
                response.Data = product;
            }
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var products = await _context.Products
                .Where(p => p.Visible && !p.Deleted)
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                .ToListAsync();

            var response = new ServiceResponse<List<Product>>()
            {
                Data = products
            };
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                .Where(p => p.Category!.Url.ToLower() == categoryUrl.ToLower() && p.Visible && !p.Deleted)
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<ProductSearchDto>> SearchProducts(string searchText, int page)
        {
            var pageResults = 2f;
            var pageCount = Math.Ceiling((await FindProductsBySearchText(searchText)).Count / pageResults);
            var products = await _context.Products
                                .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) ||
                                    p.Description.ToLower().Contains(searchText.ToLower())
                                    && p.Visible && !p.Deleted)
                                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                                .Skip((page - 1) * (int)pageResults)
                                .Take((int)pageResults)
                                .ToListAsync();

            var response = new ServiceResponse<ProductSearchDto>
            {
                Data = new ProductSearchDto
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }

        private async Task<List<Product>> FindProductsBySearchText(string searchText)
        {
            return await _context.Products
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                .Where(p => p.Title.ToLower().Contains(searchText.ToLower())
                || p.Description.ToLower().Contains(searchText.ToLower())
                && p.Visible && !p.Deleted)
                .ToListAsync();
        }
    }
}
