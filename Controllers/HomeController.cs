using System.Diagnostics;
using APIwebbanhangnhe.Models;
using APIwebbanhangnhe.Models.ViewModels;
using APIwebbanhangnhe.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIwebbanhangnhe.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPlayerCardRepository _playerCardRepository;

    public HomeController(
        ILogger<HomeController> logger,
        ICategoryRepository categoryRepository,
        IPlayerCardRepository playerCardRepository)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _playerCardRepository = playerCardRepository;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeIndexViewModel
        {
            Categories = await _categoryRepository.GetAllAsync(),
            FeaturedCards = await _playerCardRepository.GetFeaturedAsync(4)
        };

        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
