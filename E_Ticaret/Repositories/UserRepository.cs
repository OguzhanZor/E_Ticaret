using Dapper;
using E_Ticaret.Data;
using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Users WHERE IsActive = 1 ORDER BY CreatedAt DESC";
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Users WHERE Id = @Id AND IsActive = 1";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Users WHERE Email = @Email AND IsActive = 1";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Users WHERE Username = @Username AND IsActive = 1";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password AND IsActive = 1";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username, Password = password });
        }

        public async Task<User> CreateAsync(User user)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO Users (FirstName, LastName, Email, Username, Password, Phone, Address, City, Country, PostalCode, IsActive, IsAdmin, CreatedAt)
                VALUES (@FirstName, @LastName, @Email, @Username, @Password, @Phone, @Address, @City, @Country, @PostalCode, @IsActive, @IsAdmin, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            
            var id = await connection.QuerySingleAsync<int>(sql, user);
            user.Id = id;
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE Users 
                SET FirstName = @FirstName, LastName = @LastName, Email = @Email, 
                    Phone = @Phone, Address = @Address, City = @City, Country = @Country, 
                    PostalCode = @PostalCode, IsActive = @IsActive, IsAdmin = @IsAdmin, 
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            await connection.ExecuteAsync(sql, user);
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE Users SET IsActive = 0, UpdatedAt = @UpdatedAt WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, UpdatedAt = DateTime.Now });
            return rowsAffected > 0;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string newPassword)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE Users SET Password = @Password, UpdatedAt = @UpdatedAt WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = userId, Password = newPassword, UpdatedAt = DateTime.Now });
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE Users SET LastLoginAt = @LastLoginAt WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = userId, LastLoginAt = DateTime.Now });
            return rowsAffected > 0;
        }

        public async Task<int> GetTotalCountAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT COUNT(*) FROM Users WHERE IsActive = 1";
            return await connection.ExecuteScalarAsync<int>(sql);
        }
    }
}
