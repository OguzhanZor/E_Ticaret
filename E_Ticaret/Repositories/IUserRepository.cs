using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> AuthenticateAsync(string username, string password);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> ChangePasswordAsync(int userId, string newPassword);
        Task<bool> UpdateLastLoginAsync(int userId);
        Task<int> GetTotalCountAsync();
    }
}
