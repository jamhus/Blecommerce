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
        

        [HttpGet("paged")]
        public async Task<ActionResult<ServiceResponse<ProductListDto>>> GetPagedProducts([FromQuery] ProductParams productParams)
        {
            return await _productService.GetPagedProducts(productParams,false);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int id)
        {
            return await _productService.GetProductAsync(id);
        }
 
        
        [HttpGet("admin"), Authorize(Roles = "Admin")]

        public async Task<ActionResult<ServiceResponse<ProductListDto>>> GetAdminProducts([FromQuery] ProductParams productParams)
        {
            return await _productService.GetPagedProducts(productParams,true);

        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct (Product product)
        {
            return await _productService.CreateProduct(product);
        }

        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> UpdateProduct(Product product)
        {
            return await _productService.UpdateProduct(product);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int id)
        {
            return await _productService.DeleteProduct(id);
        }
    }
}
