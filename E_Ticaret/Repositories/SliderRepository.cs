using Dapper;
using E_Ticaret.Data;
using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
	public class SliderRepository : ISliderRepository
	{
		private readonly DatabaseContext _context;

		public SliderRepository(DatabaseContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Slider>> GetAllAsync()
		{
			using var connection = _context.CreateConnection();
			var sql = "SELECT * FROM Sliders ORDER BY DisplayOrder, CreatedAt DESC";
			return await connection.QueryAsync<Slider>(sql);
		}

		public async Task<IEnumerable<Slider>> GetActiveAsync()
		{
			using var connection = _context.CreateConnection();
			var sql = @"SELECT * FROM Sliders
						WHERE IsActive = 1
						AND (EndDate IS NULL OR EndDate >= @CurrentDate)
						AND StartDate <= @CurrentDate
						ORDER BY DisplayOrder, CreatedAt DESC";
			return await connection.QueryAsync<Slider>(sql, new { CurrentDate = DateTime.Now });
		}

		public async Task<Slider?> GetByIdAsync(int id)
		{
			using var connection = _context.CreateConnection();
			var sql = "SELECT * FROM Sliders WHERE Id = @Id";
			return await connection.QueryFirstOrDefaultAsync<Slider>(sql, new { Id = id });
		}

		public async Task<int> CreateAsync(Slider slider)
		{
			using var connection = _context.CreateConnection();
			var sql = @"INSERT INTO Sliders (Title, Subtitle, ButtonText, ButtonUrl, ImageUrl, MobileImageUrl, DisplayOrder, IsActive, StartDate, EndDate, CreatedAt)
						VALUES (@Title, @Subtitle, @ButtonText, @ButtonUrl, @ImageUrl, @MobileImageUrl, @DisplayOrder, @IsActive, @StartDate, @EndDate, @CreatedAt);
						SELECT CAST(SCOPE_IDENTITY() as int);";
			return await connection.ExecuteScalarAsync<int>(sql, slider);
		}

		public async Task<bool> UpdateAsync(Slider slider)
		{
			using var connection = _context.CreateConnection();
			var sql = @"UPDATE Sliders SET
						Title = @Title,
						Subtitle = @Subtitle,
						ButtonText = @ButtonText,
						ButtonUrl = @ButtonUrl,
						ImageUrl = @ImageUrl,
						MobileImageUrl = @MobileImageUrl,
						DisplayOrder = @DisplayOrder,
						IsActive = @IsActive,
						StartDate = @StartDate,
						EndDate = @EndDate,
						UpdatedAt = @UpdatedAt
						WHERE Id = @Id";
			var rows = await connection.ExecuteAsync(sql, slider);
			return rows > 0;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			using var connection = _context.CreateConnection();
			var sql = "DELETE FROM Sliders WHERE Id = @Id";
			var rows = await connection.ExecuteAsync(sql, new { Id = id });
			return rows > 0;
		}

		public async Task<int> GetTotalCountAsync()
		{
			using var connection = _context.CreateConnection();
			var sql = "SELECT COUNT(1) FROM Sliders";
			return await connection.ExecuteScalarAsync<int>(sql);
		}

		public async Task<bool> UpdateDisplayOrderAsync(int id, int displayOrder)
		{
			using var connection = _context.CreateConnection();
			var sql = "UPDATE Sliders SET DisplayOrder = @DisplayOrder WHERE Id = @Id";
			var rows = await connection.ExecuteAsync(sql, new { Id = id, DisplayOrder = displayOrder });
			return rows > 0;
		}
	}
}


