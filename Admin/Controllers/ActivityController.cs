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
    [Authorize(Roles =RoleNames.Admin)]
    public class ActivityController : Controller
    {
        private readonly ILogger<ActivityController> _logger;
        private readonly IActivityService _ativityService;
        private readonly IMapper _mapper;
        public ActivityController(ILogger<ActivityController> logger, IActivityService activityService, IMapper mapper)
        {
            _logger = logger;
            _ativityService = activityService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var model = new ActivityDataTable();
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

                var result = await _ativityService.GetActivitiesDataTableAsync(parameters);

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
                _logger.LogError(ex, "Error occurred while fetching activities for DataTable.");
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
        public async Task<IActionResult> Create(ActivityViewModel activity)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid in Create Activity action.");
                return View(activity);
            }

            try
            {
                var activityDto = _mapper.Map<ActivityDto>(activity);
                var createdActivity = await _ativityService.CreateActivityAsync(activityDto);

                if (createdActivity == null)
                {
                    _logger.LogError("Failed to create Activity.");
                    ModelState.AddModelError(string.Empty, "Failed to create Activity.");
                    return View(activity);
                }

                TempData["SuccessMessage"] = "Activity created successfully.";
                _logger.LogInformation("Activity {ActivityName} created successfully.", createdActivity.EnName);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a Activity.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating a Activity.");
                return View(activity);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActivitiesPrintTable()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                {
                    _logger.LogWarning("ActivitiesPrintTable called with no session parameters.");
                    return BadRequest("No parameters found in session.");
                }

                var results = await _ativityService.GetActivitiesDataTableAsync(
                    JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

                var mappedResults = _mapper.Map<IEnumerable<ActivityDataTable>>(results.Items);

                return PartialView("_ActivitiesPrintTable", mappedResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating ActivitiesPrintTable.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ActivitiesExcel()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                    return BadRequest("No parameters found in session.");

                var dataTableParams = JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param);
                var activities = await _ativityService.GetActivitiesDataTableAsync(dataTableParams);

                var mappedResults = _mapper.Map<IEnumerable<ActivityDataTable>>(activities.Items);

                return new JqueryDataTablesExcelResult<ActivityDataTable>(mappedResults, "Activities", "Activities");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while exporting Activities to Excel.");
                return StatusCode(500, "An error occurred while generating Excel file.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Invalid Activity Id {Id} in Update GET.", id);
                return NotFound("Activity Id is not valid.");
            }

            try
            {
                var Activity = await _ativityService.GetActivityByIdAsync(id);
                if (Activity == null)
                {
                    _logger.LogWarning("No Activity found with Id {Id}.", id);
                    return NotFound();
                }

                var mappedResult = _mapper.Map<ActivityViewModel>(Activity);
                _logger.LogInformation("Activity {Id} found successfully.", id);

                return View(mappedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Activity {Id}.", id);
                return StatusCode(500, "An error occurred while fetching the Activity.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(int id, ActivityViewModel activity)
        {
            if (id <= 0 || activity.Id != id)
            {
                _logger.LogError("Activity Id mismatch: route {RouteId}, model {ModelId}.", id, activity.Id);
                return BadRequest("Activity Id mismatch.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid while updating Activity {Id}.", id);
                return View(activity);
            }

            try
            {
                var activityDto = _mapper.Map<ActivityDto>(activity);
                var result = await _ativityService.UpdateActivityAsync(id, activityDto);

                if (result == null)
                {
                    _logger.LogError("Failed to update Activity {Id}.", id);
                    ModelState.AddModelError(string.Empty, "Failed to update Activity.");
                    return View(activity);
                }

                _logger.LogInformation("Activity {Id} updated successfully.", id);
                TempData["SuccessMessage"] = "Activity updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Activity {Id}.", id);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the Activity.");
                return View(activity);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var activity = await _ativityService.GetActivityByIdAsync(id);
            if (activity == null)
            {
                _logger.LogError("Failed To Delete Activity {ActivityId}", id);
                return Json(new { success = false});
            }

            try
            {
                await _ativityService.DeleteActivityAsync(id);
                _logger.LogInformation("Activity {Id} Deleted successfully.", id);
                return Json(new { success = true, message = "Delete Successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Deleting Activity {Id}.", id);
                return Json(new { success = false});
            }
        }
    }
}
