using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blecommerce.Shared.DTOS
{
    public class ProductListDto
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public MetaData MetaData { get; set; }
    }
}
