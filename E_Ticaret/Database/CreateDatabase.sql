-- E_Ticaret Database Creation Script
-- SQL Server 2019+ compatible

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'E_Ticaret')
BEGIN
    CREATE DATABASE E_Ticaret;
END
GO

USE E_Ticaret;
GO

-- Create Categories Table
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
        UpdatedAt DATETIME2 NULL,
        FOREIGN KEY (ParentCategoryId) REFERENCES Categories(Id)
    );
END
GO

-- Create Sliders Table
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
END
GO

-- Create Brands Table
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
END
GO

-- Create Users Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(150) NOT NULL UNIQUE,
        Username NVARCHAR(100) NOT NULL UNIQUE,
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
END
GO

-- Create Products Table
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
        UpdatedAt DATETIME2 NULL,
        FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
        FOREIGN KEY (BrandId) REFERENCES Brands(Id)
    );
END
GO

-- Create Banners Table
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
END
GO

-- Create Orders Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Orders]') AND type in (N'U'))
BEGIN
    CREATE TABLE Orders (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderNumber NVARCHAR(50) NOT NULL UNIQUE,
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
        UpdatedAt DATETIME2 NULL,
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

-- Create OrderItems Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderItems]') AND type in (N'U'))
BEGIN
    CREATE TABLE OrderItems (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderId INT NOT NULL,
        ProductId INT NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL,
        TotalPrice DECIMAL(18,2) NOT NULL,
        FOREIGN KEY (OrderId) REFERENCES Orders(Id),
        FOREIGN KEY (ProductId) REFERENCES Products(Id)
    );
END
GO

-- Create Reviews Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reviews]') AND type in (N'U'))
BEGIN
    CREATE TABLE Reviews (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ProductId INT NOT NULL,
        UserId INT NOT NULL,
        Rating INT NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
        Comment NVARCHAR(1000) NULL,
        IsApproved BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME2 NULL,
        FOREIGN KEY (ProductId) REFERENCES Products(Id),
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

-- Create Indexes for better performance
CREATE INDEX IX_Products_CategoryId ON Products(CategoryId);
CREATE INDEX IX_Products_BrandId ON Products(BrandId);
CREATE INDEX IX_Products_IsActive ON Products(IsActive);
CREATE INDEX IX_Products_IsFeatured ON Products(IsFeatured);
CREATE INDEX IX_Products_IsHot ON Products(IsHot);
CREATE INDEX IX_Products_CreatedAt ON Products(CreatedAt);

CREATE INDEX IX_Banners_IsActive ON Banners(IsActive);
CREATE INDEX IX_Banners_DisplayOrder ON Banners(DisplayOrder);
CREATE INDEX IX_Banners_StartDate ON Banners(StartDate);
CREATE INDEX IX_Banners_EndDate ON Banners(EndDate);

CREATE INDEX IX_Sliders_IsActive ON Sliders(IsActive);
CREATE INDEX IX_Sliders_DisplayOrder ON Sliders(DisplayOrder);
CREATE INDEX IX_Sliders_StartDate ON Sliders(StartDate);
CREATE INDEX IX_Sliders_EndDate ON Sliders(EndDate);

CREATE INDEX IX_Orders_UserId ON Orders(UserId);
CREATE INDEX IX_Orders_Status ON Orders(Status);
CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate);

CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
CREATE INDEX IX_OrderItems_ProductId ON OrderItems(ProductId);

CREATE INDEX IX_Reviews_ProductId ON Reviews(ProductId);
CREATE INDEX IX_Reviews_UserId ON Reviews(UserId);
CREATE INDEX IX_Reviews_IsApproved ON Reviews(IsApproved);

-- Insert Sample Data
-- Categories
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
END

-- Sample Sliders
IF NOT EXISTS (SELECT * FROM Sliders)
BEGIN
	INSERT INTO Sliders (Title, Subtitle, ButtonText, ButtonUrl, ImageUrl, DisplayOrder, IsActive, StartDate)
	VALUES
	('Don''t miss amazing grocery deals', 'Sign up for the daily newsletter', 'Subscribe', '#', '~/assets/imgs/slider/slider-1.png', 1, 1, GETDATE()),
	('Fresh Vegetables Big discount', 'Save up to 50% off on your first order', 'Shop Now', '/products', '~/assets/imgs/slider/slider-2.png', 2, 1, GETDATE());
END

-- Brands
IF NOT EXISTS (SELECT * FROM Brands WHERE Name = 'NestFood')
BEGIN
    INSERT INTO Brands (Name, Description, DisplayOrder) VALUES 
    ('NestFood', 'Premium organic food products', 1),
    ('FreshCo', 'Fresh and healthy products', 2),
    ('OrganicLife', '100% organic products', 3),
    ('HealthyChoice', 'Healthy lifestyle products', 4),
    ('PremiumBrand', 'Premium quality products', 5);
END

-- Sample Products
IF NOT EXISTS (SELECT * FROM Products WHERE Name = 'Organic Quinoa')
BEGIN
    INSERT INTO Products (Name, Description, Price, OldPrice, StockQuantity, CategoryId, BrandId, IsFeatured, IsHot) VALUES 
    ('Organic Quinoa', 'Premium organic quinoa, brown, & red rice', 28.85, 32.80, 100, 1, 1, 1, 1),
    ('Fresh Vegetables Mix', 'Organic fresh vegetables mix', 15.99, 19.99, 50, 5, 2, 1, 0),
    ('Organic Milk', 'Fresh organic whole milk', 8.99, 10.99, 75, 1, 3, 0, 1),
    ('Whole Grain Bread', 'Fresh whole grain bread', 4.99, 5.99, 60, 10, 4, 0, 0),
    ('Fresh Strawberries', 'Sweet fresh strawberries', 6.99, 8.99, 40, 5, 5, 1, 0);
END

-- Sample Banners
IF NOT EXISTS (SELECT * FROM Banners WHERE Title = 'Don''t miss amazing grocery deals')
BEGIN
    INSERT INTO Banners (Title, Subtitle, ButtonText, ButtonUrl, ImageUrl, DisplayOrder, IsActive, StartDate) VALUES 
    ('Don''t miss amazing grocery deals', 'Sign up for the daily newsletter', 'Subscribe', '#', '~/assets/imgs/slider/slider-1.png', 1, 1, GETDATE()),
    ('Fresh Vegetables Big discount', 'Save up to 50% off on your first order', 'Shop Now', '/products', '~/assets/imgs/slider/slider-2.png', 2, 1, GETDATE()),
    ('Everyday Fresh & Clean', 'Make your Breakfast Healthy and Easy', 'Shop Now', '/categories', '~/assets/imgs/banner/banner-1.png', 3, 1, GETDATE());
END

-- Sample Admin User
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (FirstName, LastName, Email, Username, Password, IsAdmin) VALUES 
    ('Admin', 'User', 'admin@eticaret.com', 'admin', 'admin123', 1);
END

PRINT 'E_Ticaret database and tables created successfully!';
GO
