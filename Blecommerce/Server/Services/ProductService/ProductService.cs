﻿using Microsoft.EntityFrameworkCore;

namespace Blecommerce.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
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
                .Include(p => p.Variants)
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
                .Where(p => p.Category!.Url.ToLower() == categoryUrl.ToLower())
                .Include(p => p.Variants)
                .ToListAsync()
        };

            return response;
        }
    }
}
