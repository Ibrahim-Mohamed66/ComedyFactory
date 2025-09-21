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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Admin.Controllers
{
    [Authorize(Roles = RoleNames.Admin)]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CountryController(
            ICountryService countryService,
            ILogger<CountryController> logger,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _countryService = countryService;
            _logger = logger;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new CountryDataTable();
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

                var result = await _countryService.GetCountiesDataTableAsync(parameters);

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
                _logger.LogError(ex, "Error occurred while fetching countries for DataTable.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CountryViewModel country)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid in Create Country action.");
                return View(country);
            }

            try
            {
                var countryDto = _mapper.Map<CountryDto>(country);
                var createdCountry = await _countryService.CreateCountryAsync(countryDto);

                if (createdCountry == null)
                {
                    _logger.LogError("Failed to create country.");
                    ModelState.AddModelError(string.Empty, "Failed to create country.");
                    return View(country);
                }

                TempData["SuccessMessage"] = "Country created successfully.";
                _logger.LogInformation("Country {CountryName} created successfully.", createdCountry.EnName);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a country.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating a country.");
                return View(country);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CountriesPrintTable()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                {
                    _logger.LogWarning("CountriesPrintTable called with no session parameters.");
                    return BadRequest("No parameters found in session.");
                }

                var results = await _countryService.GetCountiesDataTableAsync(
                    JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

                var mappedResults = _mapper.Map<IEnumerable<CountryDataTable>>(results.Items);

                return PartialView("_CountriesPrintTable", mappedResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating CountriesPrintTable.");
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
                var countries = await _countryService.GetCountiesDataTableAsync(dataTableParams);

                var mappedResults = _mapper.Map<IEnumerable<CountryDataTable>>(countries.Items);

                return new JqueryDataTablesExcelResult<CountryDataTable>(mappedResults, "Countries", "Countries");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while exporting Countries to Excel.");
                return StatusCode(500, "An error occurred while generating Excel file.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int countryId)
        {
            if (countryId <= 0)
            {
                _logger.LogError("Invalid Country Id {Id} in UpdateCountryAsync GET.", countryId);
                return NotFound("Country Id is not valid.");
            }

            try
            {
                var country = await _countryService.GetCountryByIdAsync(countryId);
                if (country == null)
                {
                    _logger.LogWarning("No country found with Id {Id}.", countryId);
                    return NotFound();
                }

                var mappedResult = _mapper.Map<CountryViewModel>(country);
                _logger.LogInformation("Country {Id} found successfully.", countryId);

                return View(mappedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching country {Id}.", countryId);
                return StatusCode(500, "An error occurred while fetching the country.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(int countryId, CountryViewModel country)
        {
            if (countryId <= 0 || country.CountryId != countryId)
            {
                _logger.LogError("Country Id mismatch: route {RouteId}, model {ModelId}.", countryId, country.CountryId);
                return BadRequest("Country Id mismatch.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid while updating Country {Id}.", countryId);
                return View(country);
            }

            try
            {
                var countryDto = _mapper.Map<CountryDto>(country);
                var result = await _countryService.UpdateCountryAsync(countryId, countryDto);

                if (result == null)
                {
                    _logger.LogError("Failed to update Country {Id}.", countryId);
                    ModelState.AddModelError(string.Empty, "Failed to update country.");
                    return View(country);
                }

                _logger.LogInformation("Country {Id} updated successfully.", countryId);
                TempData["SuccessMessage"] = "Country updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Country {Id}.", countryId);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the country.");
                return View(country);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int countryId)
        {
            var country = await _countryService.GetCountryByIdAsync(countryId);
            if (country == null)
            {
                _logger.LogError("Failed To Delete Country {countryId}", countryId);
                return Json(new { success = false });
            }

            try
            {
                await _countryService.DeleteCountryAsync(countryId);
                _logger.LogInformation("Country {Id} Deleted successfully.", countryId);
                return Json(new { success = true, message = _localizer["DeleteSuccessful"] });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Deleting Country {Id}.", countryId);
                return Json(new { success = false, message = _localizer["ErrorOccurred"] });
            }
        }
    }
}
