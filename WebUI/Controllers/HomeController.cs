using Application.IServices;
using Domain.Enums;
using IOC.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using WebUI.Models;

namespace WebUI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlockService _blockService;
        private readonly IMasterCategoryService _masterCategoryService;
        private readonly IProfessorService _professorService;
        private readonly IActivityService _activityService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger,
            IBlockService blockService,
            IMasterCategoryService masterCategoryService,
            IProfessorService professorService,
            IActivityService activityService,
            IStringLocalizer<SharedResource> localizer,
            IConfiguration configuration)
        {
            _logger = logger;
            _blockService = blockService;
            _masterCategoryService = masterCategoryService;
            _professorService = professorService;
            _activityService = activityService;
            _localizer = localizer;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var masterCategories = await _masterCategoryService.GetAllMasterCategoriesAsync();
            var activities = await _activityService.GetAllActivitiesAsync();
            var professors = await _professorService.GetAllProfessorsAsync();
            var homePage = await _blockService.GetBlockByTypeAsync(BlockType.HomePage);
            return View(new HomeViewModel
            {
                MasterCategories = masterCategories,
                Activities = activities,
                Professors = professors,
                HomePage = homePage
            });
        }

        public IActionResult Redirect()
        {
            return RedirectToAction(nameof(Index), new { language = _configuration["DefaultLanguage"] });
        }


        public async Task<IActionResult> Terms()
        {
            var termsBlock = await _activityService.GetAllActivitiesAsync();
            var homeVm = new HomeViewModel
            {
                Activities = termsBlock
            };
            return View(homeVm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
