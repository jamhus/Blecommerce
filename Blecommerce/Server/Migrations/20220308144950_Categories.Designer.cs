// <auto-generated />
using Blecommerce.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Blecommerce.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220308144950_Categories")]
    partial class Categories
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Blecommerce.Shared.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Books",
                            Url = "books"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Movies",
                            Url = "movies"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Games",
                            Url = "video-games"
                        });
                });

            modelBuilder.Entity("Blecommerce.Shared.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Description = "Amelie Muscat arbetar som barnpsykolog. På en kurs har hon träffat Laura och blivit blixtförälskad. De flyttar ihop utan att egentligen känna varandra. När Amelie ska passa Lauras treårige son gör hon ett ödesdigert misstag. Sedan är pojken försvunnen",
                            ImageUrl = "https://images.nextory.com/9789113101569.jpg?fit=clip&w=340",
                            Price = 9.99m,
                            Title = "Onda drömmar"
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 1,
                            Description = "Succéförfattaren och psykiatern Anders Hansen är tillbaka med en ny bok om hjärnan, där han reder ut varför vi mår så dåligt när vi har det så bra",
                            ImageUrl = "https://images.nextory.com/9789178873708.jpg?fit=clip&w=340",
                            Price = 9.99m,
                            Title = "Depphjärnan"
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 1,
                            Description = "Det finns böcker som är så starka att man inte kan värja sig. ”I dina händer” av Malin Persson Giolito är en sådan bok.",
                            ImageUrl = "https://images.nextory.com/9789146238393.jpg?fit=clip&w=340",
                            Price = 9.99m,
                            Title = "I dina händer"
                        });
                });

            modelBuilder.Entity("Blecommerce.Shared.Product", b =>
                {
                    b.HasOne("Blecommerce.Shared.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}
