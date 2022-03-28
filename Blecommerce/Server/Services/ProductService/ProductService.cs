using Microsoft.EntityFrameworkCore;

namespace Blecommerce.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _accessor;

        public ProductService(DataContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task<ServiceResponse<Product>> CreateProduct(Product product)
        {
            foreach (var variant in product.Variants)
            {
                variant.ProductType = null;
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new ServiceResponse<Product> { Data = product };
        }
        public async Task<ServiceResponse<Product>> UpdateProduct(Product product)
        {
            var dbProduct = await _context.Products.FindAsync(product.Id);
            if (dbProduct == null)
            {
                return new ServiceResponse<Product>
                {
                    Success = false,
                    Message = "Product not found."
                };
            }

            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Visible = product.Visible;
            dbProduct.Featured = product.Featured;

            foreach (var variant in product.Variants)
            {
                var dbVariant = await _context.ProductVariants
                    .SingleOrDefaultAsync(v => v.ProductId == variant.ProductId &&
                        v.ProductTypeId == variant.ProductTypeId);
                if (dbVariant == null)
                {
                    variant.ProductType = null;
                    _context.ProductVariants.Add(variant);
                }
                else
                {
                    dbVariant.ProductTypeId = variant.ProductTypeId;
                    dbVariant.Price = variant.Price;
                    dbVariant.OriginalPrice = variant.OriginalPrice;
                    dbVariant.Visible = variant.Visible;
                    dbVariant.Deleted = variant.Deleted;
                }
            }

            await _context.SaveChangesAsync();
            return new ServiceResponse<Product> { Data = product };
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
        {
            var dbProduct = await _context.Products.FindAsync(productId);
            if (dbProduct == null)
            {
                return new ServiceResponse<bool> { Data = false, Success = false, Message = "Product not found" };
            }
            dbProduct.Deleted = true;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int id)
        {
            Product product = null;

            if (_accessor.HttpContext.User.IsInRole("Admin"))
            {
                product = await _context.Products
                .Where(p => !p.Deleted)
                .Include(p => p.Variants.Where(v => !v.Deleted))
                .ThenInclude(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id);
            }
            else
            {
                product = await _context.Products
               .Where(p => p.Visible && !p.Deleted)
               .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
               .ThenInclude(p => p.ProductType)
               .FirstOrDefaultAsync(p => p.Id == id);
            }

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

        public async Task<ServiceResponse<ProductListDto>> GetPagedProducts(ProductParams productParams, bool admin)
        {
            var query = _context.Products
                .IsAdmin(admin)
                .Search(productParams.SearchText)
                .Filter(productParams.Categories).AsQueryable();

            var pagedResult = await PagedList<Product>.ToBagedList(query, productParams.PageNumber, productParams.PageSize);

            var result = new ProductListDto
            {
                Products = pagedResult,
                MetaData = pagedResult.MetaData,
            };

            var response = new ServiceResponse<ProductListDto>()
            {
                Data = result
            };
            return response;
        }
    }
}
