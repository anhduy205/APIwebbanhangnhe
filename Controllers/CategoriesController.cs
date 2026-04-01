using APIwebbanhangnhe.Models;
using APIwebbanhangnhe.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIwebbanhangnhe.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _categoryRepository.GetAllAsync());
    }

    public IActionResult Add()
    {
        return View(new Category());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        await _categoryRepository.AddAsync(category);
        TempData["SuccessMessage"] = "Đã thêm danh mục mới.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Display(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var category = await _categoryRepository.GetDetailsAsync(id.Value);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    public async Task<IActionResult> Update(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var category = await _categoryRepository.GetByIdAsync(id.Value);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, Category category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(category);
        }

        await _categoryRepository.UpdateAsync(category);
        TempData["SuccessMessage"] = "Đã cập nhật danh mục.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var category = await _categoryRepository.GetDetailsAsync(id.Value);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _categoryRepository.GetDetailsAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        if (await _categoryRepository.HasCardsAsync(id))
        {
            TempData["ErrorMessage"] = "Không thể xóa danh mục đang có thẻ cầu thủ.";
            return RedirectToAction(nameof(Index));
        }

        await _categoryRepository.DeleteAsync(category);
        TempData["SuccessMessage"] = "Đã xóa danh mục.";
        return RedirectToAction(nameof(Index));
    }
}
