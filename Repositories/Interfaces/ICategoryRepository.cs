using APIwebbanhangnhe.Models;

namespace APIwebbanhangnhe.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(int id);

    Task<Category?> GetDetailsAsync(int id);

    Task AddAsync(Category category);

    Task UpdateAsync(Category category);

    Task DeleteAsync(Category category);

    Task<bool> ExistsAsync(int id);

    Task<bool> HasCardsAsync(int id);
}
