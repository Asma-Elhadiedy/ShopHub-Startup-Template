
namespace myshop.BLL.DBInitializer;

public class SeedData(ILogger<SeedData> _logger, ApplicationDbContext _dbContext, RoleManager<ApplicationRole> _roleManager, UserManager<ApplicationUser> _userManager) : ISeedData
{
    public async Task SeedAsync()
    {
        try
        {
            if (!_dbContext.Database.HasPendingModelChanges())
                await _dbContext.Database.MigrateAsync();

            await SeedDefaultRoles();
            await SeedDefaultUser();
            await SeedDefaultSettings();
            await SeedDefaultCategories();
            await SeedDefaultProducts();
            await ClearOrphanedCarts();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private async Task ClearOrphanedCarts()
    {
        if (!await _dbContext.Carts.AnyAsync())
            return;

        await _dbContext.Carts.Where(c => c.ApplicationUserId == null).ExecuteDeleteAsync();
    }

    private async Task SeedDefaultCategories()
    {
        if (await _dbContext.Categories.AnyAsync())
            return;

        _dbContext.Categories.Add(new Category { Name = "Electronics", Description = "Smartphones, laptops, TVs, and electronic accessories." });
        _dbContext.Categories.Add(new Category { Name = "Computers & Laptops", Description = "Desktop computers, laptops, and computer accessories." });
        _dbContext.Categories.Add(new Category { Name = "Mobile Phones", Description = "Smartphones, feature phones, and mobile accessories." });
        _dbContext.Categories.Add(new Category { Name = "Home Appliances", Description = "Kitchen and household electrical appliances." });
        _dbContext.Categories.Add(new Category { Name = "Kitchen & Dining", Description = "Cookware, utensils, and dining essentials." });
        _dbContext.Categories.Add(new Category { Name = "Furniture", Description = "Home and office furniture for every room." });
        _dbContext.Categories.Add(new Category { Name = "Office Supplies", Description = "Stationery, printers, and office essentials." });
        _dbContext.Categories.Add(new Category { Name = "Books", Description = "Educational, fiction, and non-fiction books." });
        _dbContext.Categories.Add(new Category { Name = "Fashion", Description = "Clothing, footwear, and fashion accessories." });
        _dbContext.Categories.Add(new Category { Name = "Men's Clothing", Description = "Shirts, pants, jackets, and men's apparel." });
        _dbContext.Categories.Add(new Category { Name = "Women's Clothing", Description = "Dresses, tops, skirts, and women's fashion." });
        _dbContext.Categories.Add(new Category { Name = "Kids & Baby", Description = "Clothing, toys, and essentials for children and babies." });
        _dbContext.Categories.Add(new Category { Name = "Shoes", Description = "Casual, formal, sports, and outdoor footwear." });
        _dbContext.Categories.Add(new Category { Name = "Beauty & Personal Care", Description = "Cosmetics, skincare, and personal hygiene products." });
        _dbContext.Categories.Add(new Category { Name = "Health & Wellness", Description = "Healthcare products, vitamins, and wellness items." });
        _dbContext.Categories.Add(new Category { Name = "Sports & Fitness", Description = "Fitness equipment, sportswear, and sporting goods." });
        _dbContext.Categories.Add(new Category { Name = "Outdoor & Camping", Description = "Camping gear, hiking equipment, and outdoor accessories." });
        _dbContext.Categories.Add(new Category { Name = "Automotive", Description = "Car accessories, maintenance tools, and spare parts." });
        _dbContext.Categories.Add(new Category { Name = "Tools & Hardware", Description = "Power tools, hand tools, and construction supplies." });
        _dbContext.Categories.Add(new Category { Name = "Pet Supplies", Description = "Food, toys, and accessories for pets." });
        _dbContext.Categories.Add(new Category { Name = "Toys & Games", Description = "Educational toys, board games, and entertainment products." });
        _dbContext.Categories.Add(new Category { Name = "Gaming", Description = "Gaming consoles, accessories, and video games." });
        _dbContext.Categories.Add(new Category { Name = "Cameras & Photography", Description = "Digital cameras, lenses, and photography equipment." });
        _dbContext.Categories.Add(new Category { Name = "Audio & Headphones", Description = "Speakers, headphones, microphones, and audio devices." });
        _dbContext.Categories.Add(new Category { Name = "Smart Home", Description = "Smart lighting, security systems, and home automation devices." });
        _dbContext.Categories.Add(new Category { Name = "Garden & Outdoor", Description = "Gardening tools, outdoor furniture, and landscaping supplies." });
        _dbContext.Categories.Add(new Category { Name = "Jewelry & Watches", Description = "Luxury watches, jewelry, and fashion accessories." });
        _dbContext.Categories.Add(new Category { Name = "Groceries", Description = "Everyday food items, beverages, and household groceries." });
        _dbContext.Categories.Add(new Category { Name = "Cleaning Supplies", Description = "Household cleaning products and sanitation essentials." });
        _dbContext.Categories.Add(new Category { Name = "Gift Cards & Vouchers", Description = "Digital and physical gift cards for various brands." });

        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedDefaultProducts()
    {
        if (await _dbContext.Products.AnyAsync())
            return;

        _dbContext.Products.Add(new Product
        {
            Name = "Samsung Galaxy S25",
            Description = "Latest Samsung flagship smartphone with AMOLED display.",
            ImagePath = "uploads\\products\\c4d6b3dd-79bd-4de4-b0c5-3af68d14a3a3.jpg",
            Price = 999.99M,
            CategoryId = 3
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Dell XPS 15 Laptop",
            Description = "15-inch Intel Core Ultra laptop with 16GB RAM.",
            ImagePath = "uploads\\products\\1d0ac757-de1b-40b2-82f4-2e90576edeec.jpg",
            Price = 1899.99M,
            CategoryId = 2
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Sony WH-1000XM5",
            Description = "Wireless noise-cancelling headphones.",
            ImagePath = "uploads\\products\\de2027f4-f89e-4385-b2fc-04fe6dc3a599.webp",
            Price = 399.99M,
            CategoryId = 24
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Apple Watch Series 10",
            Description = "GPS smartwatch with health tracking.",
            ImagePath = "uploads\\products\\ca6e8102-4841-432e-820d-c7789e5ebc4e.jpg",
            Price = 499.99M,
            CategoryId = 1
        });

        _dbContext.Products.Add(new Product
        {
            Name = "LG Smart Refrigerator",
            Description = "Double-door smart refrigerator with Wi-Fi.",
            ImagePath = "uploads\\products\\d8d61b0d-e461-4029-b5b7-e03c25a9bb3b.jpg",
            Price = 1499.99M,
            CategoryId = 4
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Non-Stick Cookware Set",
            Description = "10-piece premium non-stick cookware.",
            ImagePath = "uploads\\products\\01a2ad8f-5119-4dee-b5af-1542e6ae3ea4.jpg",
            Price = 129.99M,
            CategoryId = 5
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Executive Office Chair",
            Description = "Ergonomic mesh office chair.",
            ImagePath = "uploads\\products\\cd8d41ff-6bd5-41b3-b8b2-23aa1661ce75.png",
            Price = 249.99M,
            CategoryId = 6
        });

        _dbContext.Products.Add(new Product
        {
            Name = "HP LaserJet Printer",
            Description = "Wireless monochrome laser printer.",
            ImagePath = "uploads\\products\\fa9efc8c-7876-4f8c-a364-adc87a9ad4c1.jpg",
            Price = 199.99M,
            CategoryId = 7
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Clean Code Book",
            Description = "A Handbook of Agile Software Craftsmanship.",
            ImagePath = "uploads\\products\\ab1751ba-a3c7-41a9-9eb3-83841c9c216f.jfif",
            Price = 39.99M,
            CategoryId = 8
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Men's Leather Jacket",
            Description = "Premium genuine leather jacket.",
            ImagePath = "uploads\\products\\983584ff-2ca0-4d74-858e-823e23b7542f.png",
            Price = 149.99M,
            CategoryId = 10
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Women's Summer Dress",
            Description = "Elegant floral midi dress.",
            ImagePath = "uploads\\products\\9c6d472b-eda8-4e43-9c39-9d7100279fb0.jpg",
            Price = 69.99M,
            CategoryId = 11
        });

        _dbContext.Products.Add(new Product
        {
            Name = "LEGO City Police Station",
            Description = "Creative building blocks set.",
            ImagePath = "uploads\\products\\59312556-fba7-461b-9ec4-59afb591ad94.png",
            Price = 89.99M,
            CategoryId = 12
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Nike Air Max Sneakers",
            Description = "Comfortable everyday running shoes.",
            ImagePath = "uploads\\products\\c3b6ead2-fc03-46df-8dc8-e36e2b72ea06.jpg",
            Price = 129.99M,
            CategoryId = 13
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Vitamin C Face Serum",
            Description = "Brightening skincare serum.",
            ImagePath = "uploads\\products\\e0c2d2d3-dd78-4558-98ab-efedad174bef.webp",
            Price = 24.99M,
            CategoryId = 14
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Multivitamin Tablets",
            Description = "Daily essential vitamins for adults.",
            ImagePath = "uploads\\products\\8f0f0104-576d-4480-aedd-e7688f6de993.jpg",
            Price = 19.99M,
            CategoryId = 15
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Adjustable Dumbbell Set",
            Description = "Pair of adjustable dumbbells.",
            ImagePath = "uploads\\products\\253c1293-3d9f-4406-a6aa-17f537daa663.jpg",
            Price = 299.99M,
            CategoryId = 16
        });

        _dbContext.Products.Add(new Product
        {
            Name = "4-Person Camping Tent",
            Description = "Waterproof outdoor camping tent.",
            ImagePath = "uploads\\products\\34192603-4a7a-48ba-a0d2-a6abace3d981.webp",
            Price = 179.00M,
            CategoryId = 17
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Car Vacuum Cleaner",
            Description = "Portable cordless vacuum for cars.",
            ImagePath = "uploads\\products\\e70e2316-eaf4-4661-b20f-24b4ec29924c.jpg",
            Price = 59.99M,
            CategoryId = 18
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Bosch Cordless Drill",
            Description = "20V cordless power drill kit.",
            ImagePath = "uploads\\products\\8ed7a88c-205c-4f65-88e6-24e00294a6b5.jfif",
            Price = 159.99M,
            CategoryId = 19
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Dog Food Premium 10kg",
            Description = "High-protein dry dog food.",
            ImagePath = "uploads\\products\\b5d61314-b817-4306-81ff-c3f6d2dc3d45.jpg",
            Price = 54.99M,
            CategoryId = 20
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Monopoly Board Game",
            Description = "Classic family board game.",
            ImagePath = "uploads\\products\\de02a899-185e-4157-91a1-8ebec04f6fdc.jpg",
            Price = 34.99M,
            CategoryId = 21
        });

        _dbContext.Products.Add(new Product
        {
            Name = "PlayStation 5 Console",
            Description = "Sony PlayStation 5 Slim Edition.",
            ImagePath = "uploads\\products\\ab45afe6-cee3-44a0-84a4-ba22f07a1aa7.jpg",
            Price = 549.99M,
            CategoryId = 22
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Canon EOS R10 Camera",
            Description = "Mirrorless digital camera.",
            ImagePath = "uploads\\products\\fc92c4da-1bb1-4ea9-820a-9f5620159f77.jpg",
            Price = 999.99M,
            CategoryId = 23
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Amazon Echo Dot",
            Description = "Smart speaker with Alexa.",
            ImagePath = "uploads\\products\\79d9ec2c-2641-4bd8-b761-e3499f692796.jpg",
            Price = 49.99M,
            CategoryId = 25
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Organic Olive Oil 1L",
            Description = "Extra virgin olive oil.",
            ImagePath = "uploads\\products\\436345a7-bd80-42a8-bf9d-06c9df233b99.jpg",
            Price = 18.99M,
            CategoryId = 28
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Multi-Surface Cleaner",
            Description = "Powerful household cleaning solution.",
            ImagePath = "uploads\\products\\65d89618-43d4-40fe-b69f-ff181a5df689.jpg",
            Price = 7.99M,
            CategoryId = 29
        });

        _dbContext.Products.Add(new Product
        {
            Name = "Apple iPad Air M3",
            Description = "11-inch tablet with M3 chip.",
            ImagePath = "uploads\\products\\bf17d95b-7ef3-433a-a3bc-dc0ed841341b.jpg",
            Price = 699.99M,
            CategoryId = 1
        });

        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedDefaultRoles()
    {
        if (await _roleManager.Roles.AnyAsync())
            return;

        await _roleManager.CreateAsync(new ApplicationRole { Name = ConstRoles.TechnicalSupport });
        await _roleManager.CreateAsync(new ApplicationRole { Name = ConstRoles.Admin });
        await _roleManager.CreateAsync(new ApplicationRole { Name = ConstRoles.Customer });
    }

    private async Task SeedDefaultUser()
    {
        if (await _userManager.Users.AnyAsync())
            return;

        var user = new ApplicationUser
        {
            FullName = ConstDefaultUser.FullName,
            Email = ConstDefaultUser.Email,
            UserName = ConstDefaultUser.Email,
            ImagePath = ConstPath.DefaultUserImagePath
        };

        await _userManager.CreateAsync(user, ConstDefaultUser.Password);
        await _userManager.AddToRolesAsync(user, [ConstRoles.TechnicalSupport, ConstRoles.Admin]);

        var customer = new ApplicationUser
        {
            FullName = ConstDefaultCustomer.FullName,
            Email = ConstDefaultCustomer.Email,
            UserName = ConstDefaultCustomer.Email,
            ImagePath = ConstPath.DefaultUserImagePath
        };

        await _userManager.CreateAsync(customer, ConstDefaultUser.Password);
        await _userManager.AddToRoleAsync(customer, ConstRoles.Customer);
    }

    private async Task SeedDefaultSettings()
    {
        if (await _dbContext.ApplicationSettings.AnyAsync())
            return;

        _dbContext.ApplicationSettings.Add(new()
        {
            Key = ConstApplicationSettingsKeys.FileStoragePath,
            Value = ConstPath.WWWRootPath
        });
        await _dbContext.SaveChangesAsync();
    }
}
