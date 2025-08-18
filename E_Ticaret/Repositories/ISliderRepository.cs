using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
	public interface ISliderRepository
	{
		Task<IEnumerable<Slider>> GetAllAsync();
		Task<IEnumerable<Slider>> GetActiveAsync();
		Task<Slider?> GetByIdAsync(int id);
		Task<int> CreateAsync(Slider slider);
		Task<bool> UpdateAsync(Slider slider);
		Task<bool> DeleteAsync(int id);
		Task<int> GetTotalCountAsync();
		Task<bool> UpdateDisplayOrderAsync(int id, int displayOrder);
	}
}


