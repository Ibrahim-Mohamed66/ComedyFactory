using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Domain.Enums;
using IOC.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using WebUI.Mapper;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    [Authorize]
    public class RegisterationController : Controller
    {
        private readonly ILogger<RegisterationController> _logger;
        private readonly IPersonalDataService _personalDataService;
        private readonly IDesireService _desireService;
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public RegisterationController(ILogger<RegisterationController> logger, IPersonalDataService personalDataService, IDesireService desireService, ICityService cityService, ICountryService countryService, IMapper mapper, IStringLocalizer<SharedResource> localizer)
        {
            _logger = logger;
            _personalDataService = personalDataService;
            _desireService = desireService;
            _cityService = cityService;
            _countryService = countryService;
            _mapper = mapper;
            _localizer = localizer;
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new PersonalDataViewModel();
            await LoadDropDownAsync(model);
            return View(model);
        }

        public async Task<IActionResult> Create(PersonalDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var personalDataDto = _mapper.Map<PersonalDataDto>(model);
                    await _personalDataService.CreatePersonDataAsync(personalDataDto);
                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating personal data");
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                }
            }
            await LoadDropDownAsync(model);
            return View(model);
        }
        private async Task LoadDropDownAsync(PersonalDataViewModel model)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var language = requestCulture?.RequestCulture.UICulture.TwoLetterISOLanguageName ?? "en";

            model.Desires = (await _desireService.GetAllDesiresAsync())
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = language == "ar" ? d.ArName : d.EnName
                }).ToList();

            model.Cities = (await _cityService.GetAllCitiesAsync())
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = language == "ar" ? c.ArName : c.EnName
                }).ToList();

            model.Countries = (await _countryService.GetAllCountriesAsync())
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = language == "ar" ? c.ArName : c.EnName
                }).ToList();

            model.GendersList = Enum.GetValues(typeof(Gender))
                .Cast<Gender>()
                .Select(g => new SelectListItem
                {
                    Value = g.ToString(),
                    Text = g.Localize(_localizer)
                })
                .ToList();
        }
        public IActionResult Success()
        {
            return View();
        }
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> GetCities(int countryId)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var language = requestCulture?.RequestCulture.UICulture.TwoLetterISOLanguageName ?? "en";

            var cities = await _cityService.GetCitiesByCountryId(countryId);

            var cityList = cities.Select(c => new
            {
                value = c.Id,
                text = language == "ar" ? c.ArName : c.EnName
            });

            return Json(cityList);
        }
        
    }

}
