using Dapper;
using E_Ticaret.Data;
using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _context;

        public ProductRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT p.*, c.Name as CategoryName, b.Name as BrandName
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryId = c.Id
                LEFT JOIN Brands b ON p.BrandId = b.Id
                WHERE p.IsActive = 1
                ORDER BY p.CreatedAt DESC";
            
            return await connection.QueryAsync<Product>(sql);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT p.*, c.Name as CategoryName, b.Name as BrandName
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryId = c.Id
                LEFT JOIN Brands b ON p.BrandId = b.Id
                WHERE p.Id = @Id AND p.IsActive = 1";
            
            return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT p.*, c.Name as CategoryName, b.Name as BrandName
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryId = c.Id
                LEFT JOIN Brands b ON p.BrandId = b.Id
                WHERE p.CategoryId = @CategoryId AND p.IsActive = 1
                ORDER BY p.CreatedAt DESC";
            
            return await connection.QueryAsync<Product>(sql, new { CategoryId = categoryId });
        }

        public async Task<IEnumerable<Product>> GetFeaturedAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT p.*, c.Name as CategoryName, b.Name as BrandName
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryId = c.Id
                LEFT JOIN Brands b ON p.BrandId = b.Id
                WHERE p.IsFeatured = 1 AND p.IsActive = 1
                ORDER BY p.CreatedAt DESC";
            
            return await connection.QueryAsync<Product>(sql);
        }

        public async Task<IEnumerable<Product>> GetHotAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT p.*, c.Name as CategoryName, b.Name as BrandName
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryId = c.Id
                LEFT JOIN Brands b ON p.BrandId = b.Id
                WHERE p.IsHot = 1 AND p.IsActive = 1
                ORDER BY p.CreatedAt DESC";
            
            return await connection.QueryAsync<Product>(sql);
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT p.*, c.Name as CategoryName, b.Name as BrandName
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryId = c.Id
                LEFT JOIN Brands b ON p.BrandId = b.Id
                WHERE p.IsActive = 1 AND 
                      (p.Name LIKE @SearchTerm OR p.Description LIKE @SearchTerm)
                ORDER BY p.CreatedAt DESC";
            
            var searchPattern = $"%{searchTerm}%";
            return await connection.QueryAsync<Product>(sql, new { SearchTerm = searchPattern });
        }

        public async Task<Product> CreateAsync(Product product)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO Products (Name, Description, Price, OldPrice, StockQuantity, 
                                   ImageUrl, ImageUrl2, ImageUrl3, CategoryId, BrandId, 
                                   IsActive, IsFeatured, IsHot, CreatedAt)
                VALUES (@Name, @Description, @Price, @OldPrice, @StockQuantity,
                       @ImageUrl, @ImageUrl2, @ImageUrl3, @CategoryId, @BrandId,
                       @IsActive, @IsFeatured, @IsHot, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            
            var id = await connection.QuerySingleAsync<int>(sql, product);
            product.Id = id;
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE Products 
                SET Name = @Name, Description = @Description, Price = @Price, 
                    OldPrice = @OldPrice, StockQuantity = @StockQuantity,
                    ImageUrl = @ImageUrl, ImageUrl2 = @ImageUrl2, ImageUrl3 = @ImageUrl3,
                    CategoryId = @CategoryId, BrandId = @BrandId,
                    IsActive = @IsActive, IsFeatured = @IsFeatured, IsHot = @IsHot,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            await connection.ExecuteAsync(sql, product);
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE Products SET IsActive = 0, UpdatedAt = @UpdatedAt WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, UpdatedAt = DateTime.Now });
            return rowsAffected > 0;
        }

        public async Task<int> GetTotalCountAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT COUNT(*) FROM Products WHERE IsActive = 1";
            return await connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<IEnumerable<Product>> GetPaginatedAsync(int page, int pageSize)
        {
            using var connection = _context.CreateConnection();
            var offset = (page - 1) * pageSize;
            var sql = @"
                SELECT p.*, c.Name as CategoryName, b.Name as BrandName
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryId = c.Id
                LEFT JOIN Brands b ON p.BrandId = b.Id
                WHERE p.IsActive = 1
                ORDER BY p.CreatedAt DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            
            return await connection.QueryAsync<Product>(sql, new { Offset = offset, PageSize = pageSize });
        }
    }
}
