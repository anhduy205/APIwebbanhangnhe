using APIwebbanhangnhe.Data;
using APIwebbanhangnhe.Models;
using APIwebbanhangnhe.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIwebbanhangnhe.Repositories;

public class PlayerCardRepository : IPlayerCardRepository
{
    private readonly ApplicationDbContext _context;

    public PlayerCardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PlayerCard>> GetAllAsync()
    {
        return await _context.PlayerCards
            .Include(playerCard => playerCard.Category)
            .Include(playerCard => playerCard.Images)
            .OrderByDescending(playerCard => playerCard.CreatedAt)
            .ThenBy(playerCard => playerCard.Title)
            .ToListAsync();
    }

    public async Task<List<PlayerCard>> GetFeaturedAsync(int count)
    {
        return await _context.PlayerCards
            .Include(playerCard => playerCard.Category)
            .OrderByDescending(playerCard => playerCard.Price)
            .ThenBy(playerCard => playerCard.Title)
            .Take(count)
            .ToListAsync();
    }

    public async Task<PlayerCard?> GetByIdAsync(int id)
    {
        return await _context.PlayerCards
            .Include(playerCard => playerCard.Category)
            .Include(playerCard => playerCard.Images)
            .FirstOrDefaultAsync(playerCard => playerCard.Id == id);
    }

    public async Task AddAsync(PlayerCard playerCard)
    {
        _context.PlayerCards.Add(playerCard);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PlayerCard playerCard)
    {
        _context.PlayerCards.Update(playerCard);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PlayerCard playerCard)
    {
        _context.PlayerCards.Remove(playerCard);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.PlayerCards.AnyAsync(playerCard => playerCard.Id == id);
    }
}
