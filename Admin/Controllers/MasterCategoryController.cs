using Admin.DataTable;
using Admin.ViewModels;
using Application.DTOS;
using Application.IServices;
using Application.Services;
using AutoMapper;
using Domain.StaticData;
using JqueryDataTables.ServerSide.AspNetCoreWeb.ActionResults;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Admin.Controllers
{
    [Authorize (Roles = RoleNames.Admin)]
    public class MasterCategoryController : Controller
    {
        private readonly ILogger<MasterCategoryController> _logger;
        private readonly IMasterCategoryService _masterCategoryService;
        private readonly IMapper _mapper;
        public MasterCategoryController(ILogger<MasterCategoryController> logger, IMasterCategoryService masterCategoryService, IMapper mapper)
        {
            _logger = logger;
            _masterCategoryService = masterCategoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new MasterCategoryDataTable();
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

                var result = await _masterCategoryService.GetMasterCategoriesDataTableAsync(parameters);

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
        public async Task<IActionResult> Create(MasterCategoryViewModel masterCategory)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid in Create MasterCategory action.");
                return View(masterCategory);
            }

            try
            {
                var masterCategoryDto = _mapper.Map<MasterCategoryDto>(masterCategory);
                var createdMasterCategory = await _masterCategoryService.CreateMasterCategoryAsync(masterCategoryDto);

                if (createdMasterCategory == null)
                {
                    _logger.LogError("Failed to create MasterCategory.");
                    ModelState.AddModelError(string.Empty, "Failed to create MasterCategory.");
                    return View(masterCategory);
                }

                TempData["SuccessMessage"] = "MasterCategory created successfully.";
                _logger.LogInformation("MasterCategory {MasterCategoryName} created successfully.", createdMasterCategory.EnName);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a MasterCategory.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating a MasterCategory.");
                return View(masterCategory);
            }
        }

        [HttpPost]
        public async Task<IActionResult> MasterCategoriesPrintTable()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                {
                    _logger.LogWarning("CountriesPrintTable called with no session parameters.");
                    return BadRequest("No parameters found in session.");
                }

                var results = await _masterCategoryService.GetMasterCategoriesDataTableAsync(
                    JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

                var mappedResults = _mapper.Map<IEnumerable<MasterCategoryDataTable>>(results.Items);

                return PartialView("_MasterCategoriesPrintTable", mappedResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating MasterCategorysPrintTable.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> MasterCategoriesExcel()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                    return BadRequest("No parameters found in session.");

                var dataTableParams = JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param);
                var masterCategoryies = await _masterCategoryService.GetMasterCategoriesDataTableAsync(dataTableParams);

                var mappedResults = _mapper.Map<IEnumerable<MasterCategoryDataTable>>(masterCategoryies.Items);

                return new JqueryDataTablesExcelResult<MasterCategoryDataTable>(mappedResults, "MasterCategories", "MasterCategories");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while exporting MasterCategorys to Excel.");
                return StatusCode(500, "An error occurred while generating Excel file.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Invalid MasterCategory Id {Id} in Update GET.", id);
                return NotFound("MasterCategory Id is not valid.");
            }

            try
            {
                var masterCategoryies = await _masterCategoryService.GetMasterCategoryByIdAsync(id);
                if (masterCategoryies == null)
                {
                    _logger.LogWarning("No MasterCategory found with Id {Id}.", id);
                    return NotFound();
                }

                var mappedResult = _mapper.Map<MasterCategoryViewModel>(masterCategoryies);
                _logger.LogInformation("MasterCategory {Id} found successfully.", id);

                return View(mappedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching MasterCategory {Id}.", id);
                return StatusCode(500, "An error occurred while fetching the MasterCategory.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(int id, MasterCategoryViewModel masterCategoryies)
        {
            if (id <= 0 || masterCategoryies.Id != id)
            {
                _logger.LogError("MasterCategory Id mismatch: route {RouteId}, model {ModelId}.", id, masterCategoryies.Id);
                return BadRequest("MasterCategory Id mismatch.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid while updating MasterCategory {Id}.", id);
                return View(masterCategoryies);
            }

            try
            {
                var masterCategoryDto = _mapper.Map<MasterCategoryDto>(masterCategoryies);
                var result = await _masterCategoryService.UpdateMasterCategoryAsync(id, masterCategoryDto);

                if (result == null)
                {
                    _logger.LogError("Failed to update MasterCategory {Id}.", id);
                    ModelState.AddModelError(string.Empty, "Failed to update MasterCategory.");
                    return View(masterCategoryies);
                }

                _logger.LogInformation("MasterCategory {Id} updated successfully.", id);
                TempData["SuccessMessage"] = "MasterCategory updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating MasterCategory {Id}.", id);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the MasterCategory.");
                return View(masterCategoryies);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _masterCategoryService.GetMasterCategoryByIdAsync(id);
            if (product == null)
            {
                _logger.LogError("Failed To Delete MasterCategory {MasterCategoryId}", id);
                return Json(new { success = false});
            }

            try
            {
                await _masterCategoryService.DeleteMasterCategoryAsync(id);
                _logger.LogInformation("MasterCategory {Id} Deleted successfully.", id);
                return Json(new { success = true, message = "Delete Successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Deleting MasterCategory {Id}.", id);
                return Json(new { success = false });
            }
        }
    }
}
