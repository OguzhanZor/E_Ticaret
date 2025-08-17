using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
    public interface IBannerRepository
    {
        Task<IEnumerable<Banner>> GetAllAsync();
        Task<Banner?> GetByIdAsync(int id);
        Task<IEnumerable<Banner>> GetActiveAsync();
        Task<IEnumerable<Banner>> GetActiveForDateAsync(DateTime date);
        Task<Banner> CreateAsync(Banner banner);
        Task<Banner> UpdateAsync(Banner banner);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateDisplayOrderAsync(int id, int displayOrder);
        Task<int> GetTotalCountAsync();
    }
}
