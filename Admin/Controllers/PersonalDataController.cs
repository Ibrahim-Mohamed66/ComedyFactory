using Admin.DataTable;
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
    [Authorize(Roles = RoleNames.Admin)]
    public class PersonalDataController : Controller
    {
        private readonly ILogger<PersonalDataController> _logger;
        private readonly IPersonalDataService _personalDataService;
        private readonly IMapper _mapper;
        public PersonalDataController(ILogger<PersonalDataController> logger,
            IPersonalDataService personalDataService,
            IMapper mapper)
        {
            _logger = logger;
            _personalDataService = personalDataService;
            _mapper = mapper;
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

                var result = await _personalDataService.GetPersonDatasDataTableAsync(parameters);

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
                _logger.LogError(ex, "Error occurred while fetching personal data for DataTable.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PersonalDataPrintTable()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                {
                    _logger.LogWarning("PersonalDataPrintTable called with no session parameters.");
                    return BadRequest("No parameters found in session.");
                }

                var results = await _personalDataService.GetPersonDatasDataTableAsync(
                    JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

                var mappedResults = _mapper.Map<IEnumerable<PersonalDataTable>>(results.Items);

                return PartialView("_PersonalDataPrintTable", mappedResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating PersonalDataPrintTable.");
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
                var countries = await _personalDataService.GetPersonDatasDataTableAsync(dataTableParams);

                var mappedResults = _mapper.Map<IEnumerable<PersonalDataTable>>(countries.Items);

                _logger.LogInformation("Exporting Excel File Completed Succesfully");
                return new JqueryDataTablesExcelResult<PersonalDataTable>(mappedResults, "PersonalData", "PersonalData");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while exporting PersonalData to Excel.");
                return StatusCode(500, "An error occurred while generating Excel file.");
            }
        }
    }
}
