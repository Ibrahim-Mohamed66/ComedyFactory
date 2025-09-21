using Admin.DataTable;
using Admin.ViewModels;
using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Domain.StaticData;
using JqueryDataTables.ServerSide.AspNetCoreWeb.ActionResults;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Admin.Controllers
{
    [Authorize(Roles = RoleNames.Admin)]
    public class ProfessorController : Controller
    {
        private readonly IProfessorService _professorService;
        private readonly IMasterCategoryService _masterCategoryService;
        private readonly ILogger<ProfessorController> _logger;
        private readonly IMapper _mapper;
        public ProfessorController(IProfessorService professorService, ILogger<ProfessorController> logger, IMapper mapper, IMasterCategoryService masterCategoryService)
        {
            _professorService = professorService;
            _logger = logger;
            _mapper = mapper;
            _masterCategoryService = masterCategoryService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var model = new ProfessorDataTable();
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

                var result = await _professorService.GetProfessorsDataTableAsync(parameters);

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
                _logger.LogError(ex, "Error occurred while fetching Professors for DataTable.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var MasterCategoriesList = new ProfessorViewModel
            {
                MasterCategories = await GetMasterCategoriesSelectListAsync()
            };

            return View(MasterCategoriesList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProfessorViewModel professor)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid in Create Professor action.");
                var MasterCategoriesList = new ProfessorViewModel
                {
                    MasterCategories = await GetMasterCategoriesSelectListAsync()
                };
                return View(professor);
            }

            try
            {
                var professorDto = _mapper.Map<ProfessorDto>(professor);
                var createdProfessor = await _professorService.CreateProfessorAsync(professorDto);

                if (createdProfessor == null)
                {
                    _logger.LogError("Failed to create Professor.");
                    ModelState.AddModelError(string.Empty, "Failed to create Professor.");
                    return View(professor);
                }

                TempData["SuccessMessage"] = "Professor created successfully.";
                _logger.LogInformation("Professor {ProfessorName} created successfully.", createdProfessor.EnName);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while creating a Professor.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating a Professor.");
                var MasterCategoriesList = new ProfessorViewModel
                {
                    MasterCategories = await GetMasterCategoriesSelectListAsync()
                };
                return View(professor);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProfessorsPrintTable()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                {
                    _logger.LogWarning("ProfessorsPrintTable called with no session parameters.");
                    return BadRequest("No parameters found in session.");
                }

                var results = await _professorService.GetProfessorsDataTableAsync(
                    JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

                var mappedResults = _mapper.Map<IEnumerable<ProfessorDataTable>>(results.Items);

                return PartialView("_ProfessorsPrintTable", mappedResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating ProfessorsPrintTable.");
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
                var MasterCategories = await _professorService.GetProfessorsDataTableAsync(dataTableParams);

                var mappedResults = _mapper.Map<IEnumerable<ProfessorDataTable>>(MasterCategories.Items);

                _logger.LogInformation("Exporting Excel File Completed Succesfully");
                return new JqueryDataTablesExcelResult<ProfessorDataTable>(mappedResults, "Professors", "Professors");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while exporting Professors to Excel.");
                return StatusCode(500, "An error occurred while generating Excel file.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Invalid Professor Id {Id} in Update GET.", id);
                return NotFound("Professor Id is not valid.");
            }

            try
            {
                var Professor = await _professorService.GetProfessorByIdAsync(id);
                if (Professor == null)
                {
                    _logger.LogWarning("No Professor found with Id {Id}.", id);
                    return NotFound();
                }

                var mappedResult = _mapper.Map<ProfessorViewModel>(Professor);
                mappedResult.MasterCategories = await GetMasterCategoriesSelectListAsync();
                _logger.LogInformation("Professor {Id} found successfully.", id);

                return View(mappedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Professor {Id}.", id);
                return StatusCode(500, "An error occurred while fetching the Professor.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(int id, ProfessorViewModel professor)
        {
            if (id <= 0 || professor.Id != id)
            {
                _logger.LogError("Professor Id mismatch: route {RouteId}, model {ModelId}.", id, professor.Id);
                return BadRequest("Professor Id mismatch.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid while updating Professor {Id}.", id);
                professor.MasterCategories = await GetMasterCategoriesSelectListAsync();
                return View(professor);
            }

            try
            {
                var professorDto = _mapper.Map<ProfessorDto>(professor);
                var result = await _professorService.UpdateProfessorAsync(id, professorDto);

                if (result == null)
                {
                    _logger.LogError("Failed to update Professor {Id}.", id);
                    ModelState.AddModelError(string.Empty, "Failed to update Professor.");
                    professor.MasterCategories = await GetMasterCategoriesSelectListAsync();
                    return View(professor);
                }

                _logger.LogInformation("Professor {Id} updated successfully.", id);
                TempData["SuccessMessage"] = "Professor updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Professor {Id}.", id);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the Professor.");
                professor.MasterCategories = await GetMasterCategoriesSelectListAsync();
                return View(professor);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _professorService.GetProfessorByIdAsync(id);
            if (product == null)
            {
                _logger.LogError("Failed To Delete Professor {id}", id);
                return Json(new { success = false});
            }

            try
            {
                await _professorService.DeleteProfessorAsync(id);
                _logger.LogInformation("Professor {Id} Deleted successfully.", id);
                return Json(new { success = true, message = "Delete Successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Deleting Professor {Id}.", id);
                return Json(new { success = false});
            }
        }
        private async Task<List<SelectListItem>> GetMasterCategoriesSelectListAsync()
        {
            return (await _masterCategoryService.GetAllMasterCategoriesAsync())
                   .Select(c => new SelectListItem { Text = c.EnName, Value = c.Id.ToString() })
                   .ToList();
        }
    }
}
