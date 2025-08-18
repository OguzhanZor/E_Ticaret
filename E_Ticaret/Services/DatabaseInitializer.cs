using Dapper;
using E_Ticaret.Data;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.Data.SqlClient;

namespace E_Ticaret.Services
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(DatabaseContext context, ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation("Veritabanı başlatılıyor...");
                
                // Önce master veritabanına bağlan ve E_Ticaret veritabanını oluştur
                using var masterConnection = _context.CreateMasterConnection();
                await masterConnection.OpenAsync();
                await CreateDatabaseIfNotExistsAsync(masterConnection);
                await masterConnection.CloseAsync();
                
                // Sonra E_Ticaret veritabanına bağlan ve tabloları oluştur
                using var connection = _context.CreateConnection();
                await connection.OpenAsync();
                
                // Tabloları oluştur
                await CreateTablesAsync(connection);
                
                // Örnek verileri ekle
                await SeedDataAsync(connection);
                
                _logger.LogInformation("Veritabanı başlatma tamamlandı.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Veritabanı başlatılırken hata oluştu");
                throw;
            }
        }

        private async Task CreateDatabaseIfNotExistsAsync(SqlConnection connection)
        {
            var sql = @"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'E_Ticaret')
                BEGIN
                    CREATE DATABASE E_Ticaret;
                END";
            
            await connection.ExecuteAsync(sql);
            _logger.LogInformation("Veritabanı kontrol edildi: E_Ticaret");
        }

        private async Task CreateTablesAsync(SqlConnection connection)
        {
            // Categories tablosu
            var createCategoriesTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE Categories (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(100) NOT NULL,
                        Description NVARCHAR(500) NULL,
                        ImageUrl NVARCHAR(200) NULL,
                        ParentCategoryId INT NULL,
                        IsActive BIT NOT NULL DEFAULT 1,
                        DisplayOrder INT NOT NULL DEFAULT 0,
                        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
                        UpdatedAt DATETIME2 NULL
                    );
                END";
            await connection.ExecuteAsync(createCategoriesTable);

            // Brands tablosu
            var createBrandsTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Brands]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE Brands (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(100) NOT NULL,
                        Description NVARCHAR(500) NULL,
                        LogoUrl NVARCHAR(200) NULL,
                        Website NVARCHAR(200) NULL,
                        IsActive BIT NOT NULL DEFAULT 1,
                        DisplayOrder INT NOT NULL DEFAULT 0,
                        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
                        UpdatedAt DATETIME2 NULL
                    );
                END";
            await connection.ExecuteAsync(createBrandsTable);

            // Users tablosu
            var createUsersTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE Users (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        FirstName NVARCHAR(100) NOT NULL,
                        LastName NVARCHAR(100) NOT NULL,
                        Email NVARCHAR(150) NOT NULL,
                        Username NVARCHAR(100) NOT NULL,
                        Password NVARCHAR(100) NOT NULL,
                        Phone NVARCHAR(20) NULL,
                        Address NVARCHAR(200) NULL,
                        City NVARCHAR(100) NULL,
                        Country NVARCHAR(100) NULL,
                        PostalCode NVARCHAR(20) NULL,
                        IsActive BIT NOT NULL DEFAULT 1,
                        IsAdmin BIT NOT NULL DEFAULT 0,
                        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
                        UpdatedAt DATETIME2 NULL,
                        LastLoginAt DATETIME2 NULL
                    );
                END";
            await connection.ExecuteAsync(createUsersTable);

            // Products tablosu
            var createProductsTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE Products (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(200) NOT NULL,
                        Description NVARCHAR(1000) NULL,
                        Price DECIMAL(18,2) NOT NULL,
                        OldPrice DECIMAL(18,2) NULL,
                        StockQuantity INT NOT NULL DEFAULT 0,
                        ImageUrl NVARCHAR(200) NULL,
                        ImageUrl2 NVARCHAR(200) NULL,
                        ImageUrl3 NVARCHAR(200) NULL,
                        CategoryId INT NOT NULL,
                        BrandId INT NOT NULL,
                        IsActive BIT NOT NULL DEFAULT 1,
                        IsFeatured BIT NOT NULL DEFAULT 0,
                        IsHot BIT NOT NULL DEFAULT 0,
                        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
                        UpdatedAt DATETIME2 NULL
                    );
                END";
            await connection.ExecuteAsync(createProductsTable);

            // Banners tablosu
            var createBannersTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Banners]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE Banners (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Title NVARCHAR(200) NOT NULL,
                        Subtitle NVARCHAR(500) NULL,
                        ButtonText NVARCHAR(200) NULL,
                        ButtonUrl NVARCHAR(200) NULL,
                        ImageUrl NVARCHAR(200) NOT NULL,
                        MobileImageUrl NVARCHAR(200) NULL,
                        DisplayOrder INT NOT NULL DEFAULT 0,
                        IsActive BIT NOT NULL DEFAULT 1,
                        StartDate DATETIME2 NOT NULL DEFAULT GETDATE(),
                        EndDate DATETIME2 NULL,
                        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
                        UpdatedAt DATETIME2 NULL
                    );
                END";
            await connection.ExecuteAsync(createBannersTable);

            // Sliders tablosu
            var createSlidersTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sliders]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE Sliders (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Title NVARCHAR(200) NOT NULL,
                        Subtitle NVARCHAR(500) NULL,
                        ButtonText NVARCHAR(200) NULL,
                        ButtonUrl NVARCHAR(200) NULL,
                        ImageUrl NVARCHAR(300) NOT NULL,
                        MobileImageUrl NVARCHAR(300) NULL,
                        DisplayOrder INT NOT NULL DEFAULT 0,
                        IsActive BIT NOT NULL DEFAULT 1,
                        StartDate DATETIME2 NOT NULL DEFAULT GETDATE(),
                        EndDate DATETIME2 NULL,
                        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
                        UpdatedAt DATETIME2 NULL
                    );
                END";
            await connection.ExecuteAsync(createSlidersTable);

            // Orders tablosu
            var createOrdersTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Orders]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE Orders (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        OrderNumber NVARCHAR(50) NOT NULL,
                        UserId INT NOT NULL,
                        Status NVARCHAR(100) NOT NULL DEFAULT 'Pending',
                        TotalAmount DECIMAL(18,2) NOT NULL,
                        ShippingCost DECIMAL(18,2) NOT NULL DEFAULT 0,
                        TaxAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
                        ShippingAddress NVARCHAR(200) NULL,
                        ShippingCity NVARCHAR(100) NULL,
                        ShippingCountry NVARCHAR(100) NULL,
                        ShippingPostalCode NVARCHAR(20) NULL,
                        Notes NVARCHAR(500) NULL,
                        OrderDate DATETIME2 NOT NULL DEFAULT GETDATE(),
                        ShippedDate DATETIME2 NULL,
                        DeliveredDate DATETIME2 NULL,
                        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
                        UpdatedAt DATETIME2 NULL
                    );
                END";
            await connection.ExecuteAsync(createOrdersTable);

            // OrderItems tablosu
            var createOrderItemsTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderItems]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE OrderItems (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        OrderId INT NOT NULL,
                        ProductId INT NOT NULL,
                        Quantity INT NOT NULL,
                        UnitPrice DECIMAL(18,2) NOT NULL,
                        TotalPrice DECIMAL(18,2) NOT NULL
                    );
                END";
            await connection.ExecuteAsync(createOrderItemsTable);

            // Reviews tablosu
            var createReviewsTable = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reviews]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE Reviews (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        ProductId INT NOT NULL,
                        UserId INT NOT NULL,
                        Rating INT NOT NULL,
                        Comment NVARCHAR(1000) NULL,
                        IsApproved BIT NOT NULL DEFAULT 0,
                        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
                        UpdatedAt DATETIME2 NULL
                    );
                END";
            await connection.ExecuteAsync(createReviewsTable);

            // Foreign key constraints ekle
            await AddForeignKeysAsync(connection);

            // Indexes ekle
            await AddIndexesAsync(connection);

            _logger.LogInformation("Tüm tablolar oluşturuldu/kontrol edildi.");
        }

        private async Task AddForeignKeysAsync(SqlConnection connection)
        {
            // Products -> Categories
            var addProductCategoryFK = @"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Products_Categories')
                BEGIN
                    ALTER TABLE Products ADD CONSTRAINT FK_Products_Categories 
                    FOREIGN KEY (CategoryId) REFERENCES Categories(Id);
                END";
            await connection.ExecuteAsync(addProductCategoryFK);

            // Products -> Brands
            var addProductBrandFK = @"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Products_Brands')
                BEGIN
                    ALTER TABLE Products ADD CONSTRAINT FK_Products_Brands 
                    FOREIGN KEY (BrandId) REFERENCES Brands(Id);
                END";
            await connection.ExecuteAsync(addProductBrandFK);

            // Categories -> Categories (self-reference)
            var addCategoryParentFK = @"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Categories_ParentCategory')
                BEGIN
                    ALTER TABLE Categories ADD CONSTRAINT FK_Categories_ParentCategory 
                    FOREIGN KEY (ParentCategoryId) REFERENCES Categories(Id);
                END";
            await connection.ExecuteAsync(addCategoryParentFK);

            // Orders -> Users
            var addOrderUserFK = @"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Orders_Users')
                BEGIN
                    ALTER TABLE Orders ADD CONSTRAINT FK_Orders_Users 
                    FOREIGN KEY (UserId) REFERENCES Users(Id);
                END";
            await connection.ExecuteAsync(addOrderUserFK);

            // OrderItems -> Orders
            var addOrderItemOrderFK = @"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_OrderItems_Orders')
                BEGIN
                    ALTER TABLE OrderItems ADD CONSTRAINT FK_OrderItems_Orders 
                    FOREIGN KEY (OrderId) REFERENCES Orders(Id);
                END";
            await connection.ExecuteAsync(addOrderItemOrderFK);

            // OrderItems -> Products
            var addOrderItemProductFK = @"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_OrderItems_Products')
                BEGIN
                    ALTER TABLE OrderItems ADD CONSTRAINT FK_OrderItems_Products 
                    FOREIGN KEY (ProductId) REFERENCES Products(Id);
                END";
            await connection.ExecuteAsync(addOrderItemProductFK);

            // Reviews -> Products
            var addReviewProductFK = @"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Reviews_Products')
                BEGIN
                    ALTER TABLE Reviews ADD CONSTRAINT FK_Reviews_Products 
                    FOREIGN KEY (ProductId) REFERENCES Products(Id);
                END";
            await connection.ExecuteAsync(addReviewProductFK);

            // Reviews -> Users
            var addReviewUserFK = @"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Reviews_Users')
                BEGIN
                    ALTER TABLE Reviews ADD CONSTRAINT FK_Reviews_Users 
                    FOREIGN KEY (UserId) REFERENCES Users(Id);
                END";
            await connection.ExecuteAsync(addReviewUserFK);

            _logger.LogInformation("Foreign key constraints eklendi/kontrol edildi.");
        }

        private async Task AddIndexesAsync(SqlConnection connection)
        {
            // Products indexes
            var addProductIndexes = @"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_CategoryId')
                    CREATE INDEX IX_Products_CategoryId ON Products(CategoryId);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_BrandId')
                    CREATE INDEX IX_Products_BrandId ON Products(BrandId);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_IsActive')
                    CREATE INDEX IX_Products_IsActive ON Products(IsActive);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_IsFeatured')
                    CREATE INDEX IX_Products_IsFeatured ON Products(IsFeatured);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_IsHot')
                    CREATE INDEX IX_Products_IsHot ON Products(IsHot);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_CreatedAt')
                    CREATE INDEX IX_Products_CreatedAt ON Products(CreatedAt);";
            await connection.ExecuteAsync(addProductIndexes);

            // Banners indexes
            var addBannerIndexes = @"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Banners_IsActive')
                    CREATE INDEX IX_Banners_IsActive ON Banners(IsActive);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Banners_DisplayOrder')
                    CREATE INDEX IX_Banners_DisplayOrder ON Banners(DisplayOrder);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Banners_StartDate')
                    CREATE INDEX IX_Banners_StartDate ON Banners(StartDate);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Banners_EndDate')
                    CREATE INDEX IX_Banners_EndDate ON Banners(EndDate);";
            await connection.ExecuteAsync(addBannerIndexes);

            // Sliders indexes
            var addSliderIndexes = @"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Sliders_IsActive')
                    CREATE INDEX IX_Sliders_IsActive ON Sliders(IsActive);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Sliders_DisplayOrder')
                    CREATE INDEX IX_Sliders_DisplayOrder ON Sliders(DisplayOrder);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Sliders_StartDate')
                    CREATE INDEX IX_Sliders_StartDate ON Sliders(StartDate);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Sliders_EndDate')
                    CREATE INDEX IX_Sliders_EndDate ON Sliders(EndDate);";
            await connection.ExecuteAsync(addSliderIndexes);

            // Orders indexes
            var addOrderIndexes = @"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Orders_UserId')
                    CREATE INDEX IX_Orders_UserId ON Orders(UserId);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Orders_Status')
                    CREATE INDEX IX_Orders_Status ON Orders(Status);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Orders_OrderDate')
                    CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate);";
            await connection.ExecuteAsync(addOrderIndexes);

            // OrderItems indexes
            var addOrderItemIndexes = @"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_OrderItems_OrderId')
                    CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_OrderItems_ProductId')
                    CREATE INDEX IX_OrderItems_ProductId ON OrderItems(ProductId);";
            await connection.ExecuteAsync(addOrderItemIndexes);

            // Reviews indexes
            var addReviewIndexes = @"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Reviews_ProductId')
                    CREATE INDEX IX_Reviews_ProductId ON Reviews(ProductId);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Reviews_UserId')
                    CREATE INDEX IX_Reviews_UserId ON Reviews(UserId);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Reviews_IsApproved')
                    CREATE INDEX IX_Reviews_IsApproved ON Reviews(IsApproved);";
            await connection.ExecuteAsync(addReviewIndexes);

            _logger.LogInformation("Indexes eklendi/kontrol edildi.");
        }

        private async Task SeedDataAsync(SqlConnection connection)
        {
            // Admin user ekle
            var addAdminUser = @"
                IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
                BEGIN
                    INSERT INTO Users (FirstName, LastName, Email, Username, Password, IsAdmin, IsActive) 
                    VALUES ('Admin', 'User', 'admin@eticaret.com', 'admin', 'admin123', 1, 1);
                END";
            await connection.ExecuteAsync(addAdminUser);

            // Örnek kategoriler
            var addCategories = @"
                IF NOT EXISTS (SELECT * FROM Categories WHERE Name = 'Milks and Dairies')
                BEGIN
                    INSERT INTO Categories (Name, Description, DisplayOrder) VALUES 
                    ('Milks and Dairies', 'Fresh milk and dairy products', 1),
                    ('Clothing & Beauty', 'Fashion and beauty products', 2),
                    ('Pet Foods & Toy', 'Pet supplies and toys', 3),
                    ('Baking material', 'Baking ingredients and tools', 4),
                    ('Fresh Fruit', 'Fresh fruits and vegetables', 5),
                    ('Wines & Drinks', 'Alcoholic and non-alcoholic beverages', 6),
                    ('Fresh Seafood', 'Fresh fish and seafood', 7),
                    ('Fast food', 'Quick meals and snacks', 8),
                    ('Vegetables', 'Fresh vegetables', 9),
                    ('Bread and Juice', 'Bread and juice products', 10);
                END";
            await connection.ExecuteAsync(addCategories);

            // Örnek markalar
            var addBrands = @"
                IF NOT EXISTS (SELECT * FROM Brands WHERE Name = 'NestFood')
                BEGIN
                    INSERT INTO Brands (Name, Description, DisplayOrder) VALUES 
                    ('NestFood', 'Premium organic food products', 1),
                    ('FreshCo', 'Fresh and healthy products', 2),
                    ('OrganicLife', '100% organic products', 3),
                    ('HealthyChoice', 'Healthy lifestyle products', 4),
                    ('PremiumBrand', 'Premium quality products', 5);
                END";
            await connection.ExecuteAsync(addBrands);

            // Örnek ürünler
            var addProducts = @"
                IF NOT EXISTS (SELECT * FROM Products WHERE Name = 'Organic Quinoa')
                BEGIN
                    INSERT INTO Products (Name, Description, Price, OldPrice, StockQuantity, CategoryId, BrandId, IsFeatured, IsHot) VALUES 
                    ('Organic Quinoa', 'Premium organic quinoa, brown, & red rice', 28.85, 32.80, 100, 1, 1, 1, 1),
                    ('Fresh Vegetables Mix', 'Organic fresh vegetables mix', 15.99, 19.99, 50, 5, 2, 1, 0),
                    ('Organic Milk', 'Fresh organic whole milk', 8.99, 10.99, 75, 1, 3, 0, 1),
                    ('Whole Grain Bread', 'Fresh whole grain bread', 4.99, 5.99, 60, 10, 4, 0, 0),
                    ('Fresh Strawberries', 'Sweet fresh strawberries', 6.99, 8.99, 40, 5, 5, 1, 0);
                END";
            await connection.ExecuteAsync(addProducts);

            // Örnek banner'lar
            var addBanners = @"
                IF NOT EXISTS (SELECT * FROM Banners WHERE Title = 'Don''t miss amazing grocery deals')
                BEGIN
                    INSERT INTO Banners (Title, Subtitle, ButtonText, ButtonUrl, ImageUrl, DisplayOrder, IsActive, StartDate) VALUES 
                    ('Don''t miss amazing grocery deals', 'Sign up for the daily newsletter', 'Subscribe', '#', '~/assets/imgs/slider/slider-1.png', 1, 1, GETDATE()),
                    ('Fresh Vegetables Big discount', 'Save up to 50% off on your first order', 'Shop Now', '/products', '~/assets/imgs/slider/slider-2.png', 2, 1, GETDATE()),
                    ('Everyday Fresh & Clean', 'Make your Breakfast Healthy and Easy', 'Shop Now', '/categories', '~/assets/imgs/banner/banner-1.png', 3, 1, GETDATE());
                END";
            await connection.ExecuteAsync(addBanners);

            // Örnek slider'lar
            var addSliders = @"
                IF NOT EXISTS (SELECT * FROM Sliders)
                BEGIN
                    INSERT INTO Sliders (Title, Subtitle, ButtonText, ButtonUrl, ImageUrl, DisplayOrder, IsActive, StartDate)
                    VALUES
                    ('Don''t miss amazing grocery deals', 'Sign up for the daily newsletter', 'Subscribe', '#', '~/assets/imgs/slider/slider-1.png', 1, 1, GETDATE()),
                    ('Fresh Vegetables Big discount', 'Save up to 50% off on your first order', 'Shop Now', '/products', '~/assets/imgs/slider/slider-2.png', 2, 1, GETDATE());
                END";
            await connection.ExecuteAsync(addSliders);

            _logger.LogInformation("Örnek veriler eklendi/kontrol edildi.");
        }
    }
}
