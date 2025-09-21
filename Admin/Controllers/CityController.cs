using Admin.DataTable;

using Admin.ViewModels;
using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Domain.StaticData;
using IOC.Resources;
using JqueryDataTables.ServerSide.AspNetCoreWeb.ActionResults;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Admin.Controllers
{


    [Authorize(Roles = RoleNames.Admin)]
    public class CityController : Controller
    {
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;
        private readonly ILogger<CityController> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CityController(
            ICityService cityService,
            ILogger<CityController> logger,
            IMapper mapper,
            ICountryService countryService,
            IStringLocalizer<SharedResource> localizer)
        {
            _cityService = cityService;
            _logger = logger;
            _mapper = mapper;
            _countryService = countryService;
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new CityDataTable();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] JqueryDataTablesParameters parameters)
        {
            try
            {
                HttpContext.Session.SetString(
                    nameof(JqueryDataTablesParameters),
                    JsonConvert.SerializeObject(parameters));

                var result = await _cityService.GetCitiesDataTableAsync(parameters);

                return Json(new
                {
                    draw = parameters.Draw,
                    recordsTotal = result.TotalSize,
                    recordsFiltered = result.TotalSize,
                    data = result.Items
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching cities for DataTable.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var countriesList = new CityViewModel
            {
                Countries = await GetCountriesSelectListAsync()
            };

            return View(countriesList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityViewModel city)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid in Create City action.");
                var countriesList = new CityViewModel
                {
                    Countries = await GetCountriesSelectListAsync()
                };
                return View(city);
            }

            try
            {
                var cityDto = _mapper.Map<CityDto>(city);
                var createdCity = await _cityService.CreateCityAsync(cityDto);

                if (createdCity == null)
                {
                    _logger.LogError("Failed to create City.");
                    ModelState.AddModelError(string.Empty, "Failed to create City.");
                    return View(city);
                }

                TempData["SuccessMessage"] = "City created successfully.";
                _logger.LogInformation("City {CityName} created successfully.", createdCity.EnName);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while creating a City.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating a City.");
                var countriesList = new CityViewModel
                {
                    Countries = await GetCountriesSelectListAsync()
                };
                return View(city);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CitiesPrintTable()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                {
                    _logger.LogWarning("CitiesPrintTable called with no session parameters.");
                    return BadRequest("No parameters found in session.");
                }

                var results = await _cityService.GetCitiesDataTableAsync(
                    JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

                var mappedResults = _mapper.Map<IEnumerable<CityDataTable>>(results.Items);

                return PartialView("_CitiesPrintTable", mappedResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating CitiesPrintTable.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CountriesExcel()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                    return BadRequest("No parameters found in session.");

                var dataTableParams = JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param);
                var countries = await _cityService.GetCitiesDataTableAsync(dataTableParams);

                var mappedResults = _mapper.Map<IEnumerable<CityDataTable>>(countries.Items);

                _logger.LogInformation("Exporting Excel File Completed Succesfully");
                return new JqueryDataTablesExcelResult<CityDataTable>(mappedResults, "Cities", "Cities");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while exporting Cities to Excel.");
                return StatusCode(500, "An error occurred while generating Excel file.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int cityId)
        {
            if (cityId <= 0)
            {
                _logger.LogError("Invalid City Id {Id} in Update GET.", cityId);
                return NotFound("City Id is not valid.");
            }

            try
            {
                var City = await _cityService.GetCityByIdAsync(cityId);
                if (City == null)
                {
                    _logger.LogWarning("No City found with Id {Id}.", cityId);
                    return NotFound();
                }

                var mappedResult = _mapper.Map<CityViewModel>(City);
                mappedResult.Countries = await GetCountriesSelectListAsync();
                _logger.LogInformation("City {Id} found successfully.", cityId);

                return View(mappedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching City {Id}.", cityId);
                return StatusCode(500, "An error occurred while fetching the City.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(int cityId, CityViewModel city)
        {
            if (cityId <= 0 || city.CityId != cityId)
            {
                _logger.LogError("City Id mismatch: route {RouteId}, model {ModelId}.", cityId, city.CityId);
                return BadRequest("City Id mismatch.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid while updating City {Id}.", cityId);
                city.Countries = await GetCountriesSelectListAsync();
                return View(city);
            }

            try
            {
                var CityDto = _mapper.Map<CityDto>(city);
                var result = await _cityService.UpdateCityAsync(cityId, CityDto);

                if (result == null)
                {
                    _logger.LogError("Failed to update City {Id}.", cityId);
                    ModelState.AddModelError(string.Empty, "Failed to update City.");
                    city.Countries = await GetCountriesSelectListAsync();
                    return View(city);
                }

                _logger.LogInformation("City {Id} updated successfully.", cityId);
                TempData["SuccessMessage"] = "City updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating City {Id}.", cityId);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the City.");
                city.Countries = await GetCountriesSelectListAsync();
                return View(city);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int cityId)
        {
            var city = await _cityService.GetCityByIdAsync(cityId);
            if (city == null)
            {
                _logger.LogError("Failed To Delete City {cityId}", cityId);
                return Json(new { success = false, message = _localizer["ErrorWhileDeleting"] });
            }

            try
            {
                await _cityService.DeleteCityAsync(cityId);
                _logger.LogInformation("City {Id} Deleted successfully.", cityId);
                return Json(new { success = true, message = _localizer["DeleteSuccessful"] });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Deleting City {Id}.", cityId);
                return Json(new { success = false, message = _localizer["ErrorWhileDeleting"] });
            }
        }
        private async Task<List<SelectListItem>> GetCountriesSelectListAsync()
        {
            return (await _countryService.GetAllCountriesAsync())
                   .Select(c => new SelectListItem { Text = c.EnName, Value = c.Id.ToString() })
                   .ToList();
        }

    }

}
