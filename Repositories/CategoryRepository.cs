using APIwebbanhangnhe.Data;
using APIwebbanhangnhe.Models;
using APIwebbanhangnhe.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIwebbanhangnhe.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories
            .Include(category => category.PlayerCards)
            .OrderBy(category => category.DisplayOrder)
            .ThenBy(category => category.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<Category?> GetDetailsAsync(int id)
    {
        return await _context.Categories
            .Include(category => category.PlayerCards.OrderBy(playerCard => playerCard.Price))
            .FirstOrDefaultAsync(category => category.Id == id);
    }

    public async Task AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Categories.AnyAsync(category => category.Id == id);
    }

    public async Task<bool> HasCardsAsync(int id)
    {
        return await _context.PlayerCards.AnyAsync(playerCard => playerCard.CategoryId == id);
    }
}
