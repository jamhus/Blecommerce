using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blecommerce.Server.Migrations
{
    public partial class ProductsSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { 1, "Amelie Muscat arbetar som barnpsykolog. På en kurs har hon träffat Laura och blivit blixtförälskad. De flyttar ihop utan att egentligen känna varandra. När Amelie ska passa Lauras treårige son gör hon ett ödesdigert misstag. Sedan är pojken försvunnen", "https://images.nextory.com/9789113101569.jpg?fit=clip&w=340", 9.99m, "Onda drömmar" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { 2, "Succéförfattaren och psykiatern Anders Hansen är tillbaka med en ny bok om hjärnan, där han reder ut varför vi mår så dåligt när vi har det så bra", "https://images.nextory.com/9789178873708.jpg?fit=clip&w=340", 9.99m, "Depphjärnan" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { 3, "Det finns böcker som är så starka att man inte kan värja sig. ”I dina händer” av Malin Persson Giolito är en sådan bok.", "https://images.nextory.com/9789146238393.jpg?fit=clip&w=340", 9.99m, "I dina händer" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
