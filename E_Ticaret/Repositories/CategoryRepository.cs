using Dapper;
using E_Ticaret.Data;
using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DatabaseContext _context;

        public CategoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Categories WHERE IsActive = 1 ORDER BY DisplayOrder, Name";
            return await connection.QueryAsync<Category>(sql);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Categories WHERE Id = @Id AND IsActive = 1";
            return await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Category>> GetActiveAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Categories WHERE IsActive = 1 ORDER BY DisplayOrder, Name";
            return await connection.QueryAsync<Category>(sql);
        }

        public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Categories WHERE ParentCategoryId IS NULL AND IsActive = 1 ORDER BY DisplayOrder, Name";
            return await connection.QueryAsync<Category>(sql);
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Categories WHERE ParentCategoryId = @ParentId AND IsActive = 1 ORDER BY DisplayOrder, Name";
            return await connection.QueryAsync<Category>(sql, new { ParentId = parentId });
        }

        public async Task<Category> CreateAsync(Category category)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO Categories (Name, Description, ImageUrl, ParentCategoryId, IsActive, DisplayOrder, CreatedAt)
                VALUES (@Name, @Description, @ImageUrl, @ParentCategoryId, @IsActive, @DisplayOrder, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            
            var id = await connection.QuerySingleAsync<int>(sql, category);
            category.Id = id;
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE Categories 
                SET Name = @Name, Description = @Description, ImageUrl = @ImageUrl,
                    ParentCategoryId = @ParentCategoryId, IsActive = @IsActive, 
                    DisplayOrder = @DisplayOrder, UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            await connection.ExecuteAsync(sql, category);
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE Categories SET IsActive = 0, UpdatedAt = @UpdatedAt WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, UpdatedAt = DateTime.Now });
            return rowsAffected > 0;
        }

        public async Task<int> GetProductCountAsync(int categoryId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT COUNT(*) FROM Products WHERE CategoryId = @CategoryId AND IsActive = 1";
            return await connection.ExecuteScalarAsync<int>(sql, new { CategoryId = categoryId });
        }
    }
}
