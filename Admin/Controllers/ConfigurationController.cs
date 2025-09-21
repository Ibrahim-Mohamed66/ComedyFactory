
using Admin.ViewModels;
using Application.DTOS;
using Application.IServices;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Models;
using Domain.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{

    [Authorize(Roles = RoleNames.Admin)]
    public class ConfigurationController : Controller
    {
        private readonly IConfigurationService _configService;
        private readonly ILogger<ConfigurationController> _logger;
        private readonly IMapper _mapper;

        public ConfigurationController(IConfigurationService configService, ILogger<ConfigurationController> logger, IMapper mapper)
        {
            _configService = configService;
            _logger = logger;
            _mapper = mapper;
        }
        

        public async Task<IActionResult> Index()
        {
            var configuration = await _configService.GetFirstOrDefaultAsync();
            if (configuration != null)
            {

                return RedirectToAction(nameof(Update), new { id = configuration.Id });
            }
            else
            {
                return RedirectToAction(nameof(Create));
            }
        }

        public async Task<IActionResult> Create()
        {
            if (await _configService.AnyAsync())
            {
                TempData["ErrorMessage"] = "A configuration already exists. You can only have one configuration.";
                return RedirectToAction(nameof(Update),new {id = 1});
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConfigurationViewModel configurationViewModel)
        {
            if( await _configService.AnyAsync())
            {
                TempData["ErrorMessage"] = "A configuration already exists. You can only have one configuration.";
                return RedirectToAction(nameof(Update), new { id = configurationViewModel.Id });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var configurationDto = _mapper.Map<ConfigurationDto>(configurationViewModel);
                    var createdConfig = await _configService.AddConfigurationAsync(configurationDto);

                    TempData["SuccessMessage"] = "Configuration created successfully.";
                    return RedirectToAction(nameof(Update), new { id = createdConfig.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating configuration");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the configuration.");
                }
            }
            return View(configurationViewModel);
        }

        public async Task<IActionResult> Update(int id)
        {
            var config = await _configService.GetConfigurationAsync(id);
            if(config is null)
            {
                _logger.LogError("No Configuration found with this Id {id}", id);
                return NotFound();
            }
            _logger.LogInformation("Configuration found with this Id {id} Successfully", id);
            var configViewModel = _mapper.Map<ConfigurationViewModel>(config);
            return View(configViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,ConfigurationViewModel configurationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingConfig = await _configService.GetConfigurationAsync(id);
                    if(existingConfig is null)
                    {
                        _logger.LogError("No Configuration found with this Id {id}", configurationViewModel.Id);
                        return NotFound();
                    }
                    var configDto = _mapper.Map<ConfigurationDto>(existingConfig);
                    var result = await _configService.UpdateConfigurationAsync(id,configDto);
                    if(result is not null)
                    {
                        _logger.LogInformation("Configuration Updated Successfully");
                        return View(configurationViewModel);
                    }
                }
                return View(configurationViewModel);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while creating a Configuration.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating a Configuration.");

                return View(configurationViewModel);
            }
        }


    }
}
