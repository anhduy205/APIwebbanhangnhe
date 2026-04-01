using APIwebbanhangnhe.Models;

namespace APIwebbanhangnhe.Repositories.Interfaces;

public interface IPlayerCardRepository
{
    Task<List<PlayerCard>> GetAllAsync();

    Task<List<PlayerCard>> GetFeaturedAsync(int count);

    Task<PlayerCard?> GetByIdAsync(int id);

    Task AddAsync(PlayerCard playerCard);

    Task UpdateAsync(PlayerCard playerCard);

    Task DeleteAsync(PlayerCard playerCard);

    Task<bool> ExistsAsync(int id);
}
