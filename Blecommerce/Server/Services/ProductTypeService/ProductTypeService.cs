namespace Blecommerce.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext _context;

        public ProductTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
        {
            productType.Editing = productType.ISNew = false;
            _context.Add(productType);
            await _context.SaveChangesAsync();
            return await GetProductTypes();
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
        {
            var productTypes = await _context.ProductTypes.ToListAsync();
            return new ServiceResponse<List<ProductType>> { Data = productTypes };
        }

        public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
        {
            ProductType dbtype = await GetTypeById(productType.Id);
            if (dbtype == null)
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Success = false,
                    Message = "Type not found"
                };
            }
            dbtype.Name = productType.Name;
            await _context.SaveChangesAsync();
            return await GetProductTypes();
        }
        private async Task<ProductType> GetTypeById(int id) => await _context.ProductTypes.FindAsync(id);

    }
}
