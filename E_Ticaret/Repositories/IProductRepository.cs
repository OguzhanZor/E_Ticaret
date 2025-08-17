using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetFeaturedAsync();
        Task<IEnumerable<Product>> GetHotAsync();
        Task<IEnumerable<Product>> SearchAsync(string searchTerm);
        Task<Product> CreateAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<IEnumerable<Product>> GetPaginatedAsync(int page, int pageSize);
    }
}
