using APIwebbanhangnhe.Models;
using APIwebbanhangnhe.Models.ViewModels;
using APIwebbanhangnhe.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace APIwebbanhangnhe.Controllers;

public class PlayerCardsController : Controller
{
    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp", ".svg" };

    private const string DefaultCardImage = "/images/player-cards/default-card.svg";
    private const string UploadRoot = "images/player-cards/uploads";

    private readonly ICategoryRepository _categoryRepository;
    private readonly IPlayerCardRepository _playerCardRepository;
    private readonly IWebHostEnvironment _environment;

    public PlayerCardsController(
        IPlayerCardRepository playerCardRepository,
        ICategoryRepository categoryRepository,
        IWebHostEnvironment environment)
    {
        _playerCardRepository = playerCardRepository;
        _categoryRepository = categoryRepository;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _playerCardRepository.GetAllAsync());
    }

    public async Task<IActionResult> Add()
    {
        return View(await BuildFormViewModelAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(PlayerCardFormViewModel model)
    {
        if (!ValidateFiles(model))
        {
            model.Categories = await GetCategorySelectListAsync();
            return View(model);
        }

        var imageUrl = await ResolvePrimaryImageAsync(model.ThumbnailFile, model.ImageUrl, null);
        var playerCard = model.ToEntity(imageUrl);

        var galleryUrls = await SaveGalleryImagesAsync(model.GalleryFiles);
        foreach (var galleryUrl in galleryUrls)
        {
            playerCard.Images.Add(new PlayerCardImage { Url = galleryUrl });
        }

        await _playerCardRepository.AddAsync(playerCard);
        TempData["SuccessMessage"] = "ÄÃ£ thÃªm tháº» cáº§u thá»§ má»›i.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Display(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var playerCard = await _playerCardRepository.GetByIdAsync(id.Value);
        if (playerCard is null)
        {
            return NotFound();
        }

        return View(playerCard);
    }

    public async Task<IActionResult> Update(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var playerCard = await _playerCardRepository.GetByIdAsync(id.Value);
        if (playerCard is null)
        {
            return NotFound();
        }

        return View(await BuildFormViewModelAsync(playerCard));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, PlayerCardFormViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var playerCard = await _playerCardRepository.GetByIdAsync(id);
        if (playerCard is null)
        {
            return NotFound();
        }

        if (!ValidateFiles(model))
        {
            model.Categories = await GetCategorySelectListAsync();
            model.CurrentImageUrl = playerCard.ImageUrl;
            model.ExistingImages = playerCard.Images.ToList();
            return View(model);
        }

        var imageUrl = await ResolvePrimaryImageAsync(model.ThumbnailFile, model.ImageUrl, playerCard.ImageUrl);
        if (!string.Equals(imageUrl, playerCard.ImageUrl, StringComparison.OrdinalIgnoreCase))
        {
            DeleteUploadedFile(playerCard.ImageUrl);
        }

        model.ApplyToEntity(playerCard, imageUrl);

        var galleryUrls = await SaveGalleryImagesAsync(model.GalleryFiles);
        foreach (var galleryUrl in galleryUrls)
        {
            playerCard.Images.Add(new PlayerCardImage { Url = galleryUrl });
        }

        await _playerCardRepository.UpdateAsync(playerCard);
        TempData["SuccessMessage"] = "ÄÃ£ cáº­p nháº­t tháº» cáº§u thá»§.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var playerCard = await _playerCardRepository.GetByIdAsync(id.Value);
        if (playerCard is null)
        {
            return NotFound();
        }

        return View(playerCard);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var playerCard = await _playerCardRepository.GetByIdAsync(id);
        if (playerCard is null)
        {
            return NotFound();
        }

        DeleteUploadedFile(playerCard.ImageUrl);
        foreach (var image in playerCard.Images)
        {
            DeleteUploadedFile(image.Url);
        }

        await _playerCardRepository.DeleteAsync(playerCard);
        TempData["SuccessMessage"] = "ÄÃ£ xÃ³a tháº» cáº§u thá»§.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<PlayerCardFormViewModel> BuildFormViewModelAsync(PlayerCard? playerCard = null)
    {
        var categories = await GetCategorySelectListAsync();
        if (playerCard is null)
        {
            return new PlayerCardFormViewModel
            {
                Categories = categories,
                ImageUrl = DefaultCardImage,
                CurrentImageUrl = DefaultCardImage
            };
        }

        return PlayerCardFormViewModel.FromEntity(playerCard, categories);
    }

    private async Task<List<SelectListItem>> GetCategorySelectListAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories
            .Select(category => new SelectListItem(category.Name, category.Id.ToString()))
            .ToList();
    }

    private bool ValidateFiles(PlayerCardFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return false;
        }

        if (model.ThumbnailFile is not null && !IsAllowedFile(model.ThumbnailFile.FileName))
        {
            ModelState.AddModelError(nameof(model.ThumbnailFile), "Chá»‰ cháº¥p nháº­n file áº£nh JPG, PNG, WEBP hoáº·c SVG.");
        }

        if (model.GalleryFiles.Any(file => !IsAllowedFile(file.FileName)))
        {
            ModelState.AddModelError(nameof(model.GalleryFiles), "Má»™t hoáº·c nhiá»u file gallery khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
        }

        return ModelState.IsValid;
    }

    private static bool IsAllowedFile(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return AllowedExtensions.Contains(extension);
    }

    private async Task<string> ResolvePrimaryImageAsync(IFormFile? file, string? imageUrl, string? currentImageUrl)
    {
        if (file is not null && file.Length > 0)
        {
            return await SaveImageAsync(file, "thumb");
        }

        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            return imageUrl.Trim();
        }

        if (!string.IsNullOrWhiteSpace(currentImageUrl))
        {
            return currentImageUrl;
        }

        return DefaultCardImage;
    }

    private async Task<List<string>> SaveGalleryImagesAsync(IEnumerable<IFormFile> files)
    {
        var urls = new List<string>();

        foreach (var file in files.Where(file => file.Length > 0))
        {
            urls.Add(await SaveImageAsync(file, "gallery"));
        }

        return urls;
    }

    private async Task<string> SaveImageAsync(IFormFile file, string folderName)
    {
        var extension = Path.GetExtension(file.FileName);
        var uniqueName = $"{Guid.NewGuid():N}{extension}";
        var relativeFolder = Path.Combine(UploadRoot, folderName);
        var absoluteFolder = Path.Combine(_environment.WebRootPath, relativeFolder);

        Directory.CreateDirectory(absoluteFolder);

        var absolutePath = Path.Combine(absoluteFolder, uniqueName);
        await using var fileStream = new FileStream(absolutePath, FileMode.Create);
        await file.CopyToAsync(fileStream);

        return "/" + Path.Combine(relativeFolder, uniqueName).Replace("\\", "/");
    }

    private void DeleteUploadedFile(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return;
        }

        if (!imageUrl.StartsWith("/images/player-cards/uploads/", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var relativePath = imageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var absolutePath = Path.Combine(_environment.WebRootPath, relativePath);

        if (System.IO.File.Exists(absolutePath))
        {
            System.IO.File.Delete(absolutePath);
        }
    }
}

