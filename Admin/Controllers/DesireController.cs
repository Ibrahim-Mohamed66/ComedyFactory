using Admin.DataTable;
using Admin.ViewModels;
using Application.DTOS;
using Application.IServices;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Domain.StaticData;
using JqueryDataTables.ServerSide.AspNetCoreWeb.ActionResults;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Admin.Controllers
{
    [Authorize(Roles = RoleNames.Admin)]
    public class DesireController : Controller
    {
        private readonly ILogger<DesireController> _logger;
        private readonly IDesireService _desireService;
        private readonly IMapper _mapper;

        public DesireController(ILogger<DesireController> logger, IDesireService desireService, IMapper mapper)
        {
            _logger = logger;
            _desireService = desireService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new DesireDataTable();
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

                var result = await _desireService.GetDesiresDataTableAsync(parameters);

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
        public async Task<IActionResult> Create(DesireViewModel desire)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid in Create Desire action.");
                return View(desire);
            }

            try
            {
                var desireDto = _mapper.Map<DesireDto>(desire);
                var createdDesire = await _desireService.CreateDesireAsync(desireDto);

                if (createdDesire == null)
                {
                    _logger.LogError("Failed to create Desire.");
                    ModelState.AddModelError(string.Empty, "Failed to create Desire.");
                    return View(desire);
                }

                TempData["SuccessMessage"] = "Desire created successfully.";
                _logger.LogInformation("Desire {DesireName} created successfully.", createdDesire.EnName);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a Desire.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating a Desire.");
                return View(desire);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DesiresPrintTable()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                {
                    _logger.LogWarning("CountriesPrintTable called with no session parameters.");
                    return BadRequest("No parameters found in session.");
                }

                var results = await _desireService.GetDesiresDataTableAsync(
                    JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

                var mappedResults = _mapper.Map<IEnumerable<DesireDataTable>>(results.Items);

                return PartialView("_DesiresPrintTable", mappedResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating DesiresPrintTable.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DesiresExcel()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                    return BadRequest("No parameters found in session.");

                var dataTableParams = JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param);
                var desires = await _desireService.GetDesiresDataTableAsync(dataTableParams);

                var mappedResults = _mapper.Map<IEnumerable<DesireDataTable>>(desires.Items);

                return new JqueryDataTablesExcelResult<DesireDataTable>(mappedResults, "Desires", "Desires");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while exporting Desires to Excel.");
                return StatusCode(500, "An error occurred while generating Excel file.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Invalid Desire Id {Id} in Update GET.", id);
                return NotFound("Desire Id is not valid.");
            }

            try
            {
                var desire = await _desireService.GetDesireByIdAsync(id);
                if (desire == null)
                {
                    _logger.LogWarning("No Desire found with Id {Id}.", id);
                    return NotFound();
                }

                var mappedResult = _mapper.Map<DesireViewModel>(desire);
                _logger.LogInformation("Desire {Id} found successfully.", id);

                return View(mappedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Desire {Id}.", id);
                return StatusCode(500, "An error occurred while fetching the Desire.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(int id, DesireViewModel desire)
        {
            if (id <= 0 || desire.Id != id)
            {
                _logger.LogError("Desire Id mismatch: route {RouteId}, model {ModelId}.", id, desire.Id);
                return BadRequest("Desire Id mismatch.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid while updating Desire {Id}.", id);
                return View(desire);
            }

            try
            {
                var DesireDto = _mapper.Map<DesireDto>(desire);
                var result = await _desireService.UpdateDesireAsync(id, DesireDto);

                if (result == null)
                {
                    _logger.LogError("Failed to update Desire {Id}.", id);
                    ModelState.AddModelError(string.Empty, "Failed to update Desire.");
                    return View(desire);
                }

                _logger.LogInformation("Desire {Id} updated successfully.", id);
                TempData["SuccessMessage"] = "Desire updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Desire {Id}.", id);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the Desire.");
                return View(desire);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var desire = await _desireService.GetDesireByIdAsync(id);
            if (desire == null)
            {
                _logger.LogError("Failed To Delete Desire {DesireId}", id);
                return Json(new { success = false, message = "Error while deleting" });
            }

            try
            {
                await _desireService.DeleteDesireAsync(id);
                _logger.LogInformation("Desire {Id} Deleted successfully.", id);
                return Json(new { success = true, message = "Delete Successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Delrting Desire {Id}.", id);
                return Json(new { success = false });
            }
        }
    }
}
