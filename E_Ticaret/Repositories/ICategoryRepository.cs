using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetActiveAsync();
        Task<IEnumerable<Category>> GetMainCategoriesAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
        Task<int> GetProductCountAsync(int categoryId);
    }
}
