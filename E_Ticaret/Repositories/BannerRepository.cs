using Dapper;
using E_Ticaret.Data;
using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
    public class BannerRepository : IBannerRepository
    {
        private readonly DatabaseContext _context;

        public BannerRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Banner>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Banners ORDER BY DisplayOrder, CreatedAt DESC";
            return await connection.QueryAsync<Banner>(sql);
        }

        public async Task<Banner?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Banners WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Banner>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Banner>> GetActiveAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT * FROM Banners 
                WHERE IsActive = 1 
                AND (EndDate IS NULL OR EndDate >= @CurrentDate)
                AND StartDate <= @CurrentDate
                ORDER BY DisplayOrder, CreatedAt DESC";
            
            return await connection.QueryAsync<Banner>(sql, new { CurrentDate = DateTime.Now });
        }

        public async Task<IEnumerable<Banner>> GetActiveForDateAsync(DateTime date)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT * FROM Banners 
                WHERE IsActive = 1 
                AND (EndDate IS NULL OR EndDate >= @Date)
                AND StartDate <= @Date
                ORDER BY DisplayOrder, CreatedAt DESC";
            
            return await connection.QueryAsync<Banner>(sql, new { Date = date });
        }

        public async Task<Banner> CreateAsync(Banner banner)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO Banners (Title, Subtitle, ButtonText, ButtonUrl, ImageUrl, MobileImageUrl, 
                                   DisplayOrder, IsActive, StartDate, EndDate, CreatedAt)
                VALUES (@Title, @Subtitle, @ButtonText, @ButtonUrl, @ImageUrl, @MobileImageUrl,
                       @DisplayOrder, @IsActive, @StartDate, @EndDate, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            
            var id = await connection.QuerySingleAsync<int>(sql, banner);
            banner.Id = id;
            return banner;
        }

        public async Task<Banner> UpdateAsync(Banner banner)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE Banners 
                SET Title = @Title, Subtitle = @Subtitle, ButtonText = @ButtonText, 
                    ButtonUrl = @ButtonUrl, ImageUrl = @ImageUrl, MobileImageUrl = @MobileImageUrl,
                    DisplayOrder = @DisplayOrder, IsActive = @IsActive, StartDate = @StartDate, 
                    EndDate = @EndDate, UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            await connection.ExecuteAsync(sql, banner);
            return banner;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM Banners WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateDisplayOrderAsync(int id, int displayOrder)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE Banners SET DisplayOrder = @DisplayOrder, UpdatedAt = @UpdatedAt WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, DisplayOrder = displayOrder, UpdatedAt = DateTime.Now });
            return rowsAffected > 0;
        }

        public async Task<int> GetTotalCountAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT COUNT(*) FROM Banners";
            return await connection.ExecuteScalarAsync<int>(sql);
        }
    }
}
