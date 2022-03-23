using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blecommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]  
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            return await _productService.GetProductsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int id)
        {
            return await _productService.GetProductAsync(id);
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl)
        {
            return await _productService.GetProductsByCategory(categoryUrl);
        }

        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchDto>>> SearchProduct(string searchText, int page = 1)
        {
            return await _productService.SearchProducts(searchText,page);
        }   
        
        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
        {
            return await _productService.GetFeaturedProducts();
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]

        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProducts()
        {
            return await _productService.GetAdminProducts();
        }
    }
}
