using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blecommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static List<Product> Products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Title = "Onda drömmar",
                Description = "Amelie Muscat arbetar som barnpsykolog. På en kurs har hon träffat Laura och blivit blixtförälskad. De flyttar ihop utan att egentligen känna varandra. När Amelie ska passa Lauras treårige son gör hon ett ödesdigert misstag. Sedan är pojken försvunnen",
                ImageUrl = "https://images.nextory.com/9789113101569.jpg?fit=clip&w=340",
                Price = 9.99m
            },
            new Product
            {
                Id = 2,
                Title ="Depphjärnan",
                Description = "Succéförfattaren och psykiatern Anders Hansen är tillbaka med en ny bok om hjärnan, där han reder ut varför vi mår så dåligt när vi har det så bra",
                ImageUrl = "https://images.nextory.com/9789178873708.jpg?fit=clip&w=340",
                Price = 9.99m
            },
            new Product
            {
                Id = 3,
                Title ="I dina händer",
                Description = "Det finns böcker som är så starka att man inte kan värja sig. ”I dina händer” av Malin Persson Giolito är en sådan bok.",
                ImageUrl = "https://images.nextory.com/9789146238393.jpg?fit=clip&w=340",
                Price = 9.99m
            }
        };
        
        [HttpGet]  
        public async Task<IActionResult> GetProducts()
        {
            return Ok(Products);
        }
    }
}
