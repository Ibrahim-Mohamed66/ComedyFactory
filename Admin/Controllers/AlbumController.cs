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
    [Authorize(Roles = RoleNames.Admin)]
    public class AlbumController : Controller
    {
        private readonly ILogger<AlbumController> _logger;
        private readonly IMapper _mapper;
        private readonly IAlbumService _albumService;
        private readonly IAlbumMediaService _albumMediaService;


        public AlbumController(ILogger<AlbumController> logger, IMapper mapper, IAlbumService albumService, IAlbumMediaService albumMediaService)
        {
            _logger = logger;
            _mapper = mapper;
            _albumService = albumService;
            _albumMediaService = albumMediaService;
        }
        public IActionResult Index()
        {
            var model = new AlbumDataTable();
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

                var result = await _albumService.GetAlbumsDataTableAsync(parameters);

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
                _logger.LogError(ex, "Error occurred while fetching albums for DataTable.");
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
        public async Task<IActionResult> Create(AlbumViewModel Album)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid in Create Album action.");
                return View(Album);
            }

            try
            {
                var AlbumDto = _mapper.Map<AlbumDto>(Album);
                var createdAlbum = await _albumService.CreateAlbumAsync(AlbumDto);

                if (createdAlbum == null)
                {
                    _logger.LogError("Failed to create Album.");
                    ModelState.AddModelError(string.Empty, "Failed to create Album.");
                    return View(Album);
                }

                TempData["SuccessMessage"] = "Album created successfully.";
                _logger.LogInformation("Album {AlbumName} created successfully.", createdAlbum.EnName);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a Album.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating a Album.");
                return View(Album);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AlbumsPrintTable()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                {
                    _logger.LogWarning("AlbumsPrintTable called with no session parameters.");
                    return BadRequest("No parameters found in session.");
                }

                var results = await _albumService.GetAlbumsDataTableAsync(
                    JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

                var mappedResults = _mapper.Map<IEnumerable<AlbumDataTable>>(results.Items);

                return PartialView("_AlbumsPrintTable", mappedResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating AlbumsPrintTable.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AlbumsExcel()
        {
            try
            {
                var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));
                if (string.IsNullOrEmpty(param))
                    return BadRequest("No parameters found in session.");

                var dataTableParams = JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param);
                var countries = await _albumService.GetAlbumsDataTableAsync(dataTableParams);

                var mappedResults = _mapper.Map<IEnumerable<AlbumDataTable>>(countries.Items);

                return new JqueryDataTablesExcelResult<AlbumDataTable>(mappedResults, "Albums", "Albums");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while exporting Albums to Excel.");
                return StatusCode(500, "An error occurred while generating Excel file.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int albumId)
        {
            if (albumId <= 0)
            {
                _logger.LogError("Invalid Album Id {Id} in UpdateAlbumAsync GET.", albumId);
                return NotFound("Album Id is not valid.");
            }

            try
            {
                var album = await _albumService.GetAlbumByIdAsync(albumId);
                if (album == null)
                {
                    _logger.LogWarning("No Album found with Id {Id}.",albumId);
                    return NotFound();
                }

                var mappedResult = _mapper.Map<AlbumViewModel>(album);
                _logger.LogInformation("Album {Id} found successfully.", albumId);

                return View(mappedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Album {Id}.", albumId);
                return StatusCode(500, "An error occurred while fetching the Album.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(int id, AlbumViewModel album)
        {
            if (id <= 0 || album.Id != id)
            {
                _logger.LogError("Album Id mismatch: route {RouteId}, model {ModelId}.", id, album.Id);
                return BadRequest("Album Id mismatch.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid while updating Album {Id}.", id);
                return View(album);
            }

            try
            {
                var albumDto = _mapper.Map<AlbumDto>(album);
                var result = await _albumService.UpdateAlbumAsync(id, albumDto);

                if (result == null)
                {
                    _logger.LogError("Failed to update Album {Id}.", id);
                    ModelState.AddModelError(string.Empty, "Failed to update Album.");
                    return View(album);
                }

                _logger.LogInformation("Album {Id} updated successfully.", id);
                TempData["SuccessMessage"] = "Album updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Album {Id}.", id);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the Album.");
                return View(album);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int albumId)
        {
            var album = await _albumService.GetAlbumByIdAsync(albumId);
            if (album == null)
            {
                _logger.LogError("Failed To Delete Album {AlbumId}", albumId);
                return Json(new { success = false });
            }

            try
            {
                await _albumService.DeleteAlbumAsync(albumId);
                _logger.LogInformation("Album {Id} Deleted successfully.", albumId);
                return Json(new { success = true, message = "Delete Successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Deleting Album {Id}.", albumId);
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public IActionResult AddMedia(int albumId)
        {
            if (albumId <= 0)
            {
                _logger.LogWarning("Invalid AlbumId {AlbumId} in AddMedia GET.", albumId);
                return BadRequest("Invalid AlbumId.");
            }

            var model = new AlbumMediaViewModel
            {
                AlbumId = albumId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMedia(AlbumMediaViewModel albumMediaView)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid in Add AlbumMedia action.");
                return View(albumMediaView);
            }

            try
            {
                var albumMediaDto = _mapper.Map<AlbumMediaDto>(albumMediaView);
                var createdAlbumMedia = await _albumMediaService.CreateAlbumMediaAsync(albumMediaDto);

                if (createdAlbumMedia == null)
                {
                    _logger.LogError("Failed to create AlbumMedia.");
                    ModelState.AddModelError(string.Empty, "Failed to create Album media.");
                    return View(albumMediaView);
                }

                TempData["SuccessMessage"] = "Album media created successfully.";
                _logger.LogInformation("Media added to Album {AlbumId} successfully.", createdAlbumMedia.AlbumId);

                // Redirect to MediaTable of the album
                return RedirectToAction(nameof(MediaTable), new { albumId = createdAlbumMedia.AlbumId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating AlbumMedia for Album {AlbumId}.", albumMediaView.AlbumId);
                ModelState.AddModelError(string.Empty, "An error occurred while creating Album media.");
                return View(albumMediaView);
            }
        }

        
        public IActionResult MediaTable(int albumId)
        {
            var model = new AlbumMediaDataTable();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> MediaTable([FromQuery] int albumId, [FromBody] JqueryDataTablesParameters parameters)
        {
            if (albumId <= 0)
            {
                _logger.LogWarning("Invalid AlbumId {AlbumId} in GetMediaTable.", albumId);
                return BadRequest("Invalid AlbumId.");
            }

            try
            {
               
                HttpContext.Session.SetString(
                    $"MediaTable_{albumId}",
                    JsonConvert.SerializeObject(parameters));

                var result = await _albumMediaService.GetAlbumMediasDataTableAsync(albumId, parameters);

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
                _logger.LogError(ex, "Error occurred while fetching media for Album {AlbumId}.", albumId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateMedia(int mediaId)
        {
            if (mediaId <= 0)
            {
                _logger.LogWarning("Invalid MediaId {MediaId} in UpdateMedia GET.", mediaId);
                return BadRequest("Invalid MediaId.");
            }

            try
            {
                var media = await _albumMediaService.GetAlbumMediaByIdAsync(mediaId);
                if (media == null)
                {
                    _logger.LogWarning("No AlbumMedia found with Id {MediaId}.", mediaId);
                    return NotFound();
                }

                var mappedResult = _mapper.Map<AlbumMediaViewModel>(media);
                _logger.LogInformation("AlbumMedia {Id} found successfully.", mediaId);

                return View(mappedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching AlbumMedia {Id}.", mediaId);
                return StatusCode(500, "An error occurred while fetching the AlbumMedia.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMedia(int id, AlbumMediaViewModel mediaVm)
        {
            if (id <= 0 || mediaVm.Id != id)
            {
                _logger.LogError("AlbumMedia Id mismatch: route {RouteId}, model {ModelId}.", id, mediaVm.Id);
                return BadRequest("AlbumMedia Id mismatch.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid while updating AlbumMedia {Id}.", id);
                return View(mediaVm);
            }

            try
            {
                var mediaDto = _mapper.Map<AlbumMediaDto>(mediaVm);
                var result = await _albumMediaService.UpdateAlbumMediaAsync(id, mediaDto);

                if (result == null)
                {
                    _logger.LogError("Failed to update AlbumMedia {Id}.", id);
                    ModelState.AddModelError(string.Empty, "Failed to update AlbumMedia.");
                    return View(mediaVm);
                }

                _logger.LogInformation("AlbumMedia {Id} updated successfully.", id);
                TempData["SuccessMessage"] = "Album media updated successfully.";

                return RedirectToAction(nameof(MediaTable), new { albumId = result.AlbumId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating AlbumMedia {Id}.", id);
                ModelState.AddModelError(string.Empty, "An error occurred while updating AlbumMedia.");
                return View(mediaVm);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMedia(int mediaId)
        {
            if (mediaId <= 0)
            {
                _logger.LogWarning("Invalid MediaId {MediaId} in DeleteMedia.", mediaId);
                return BadRequest("Invalid MediaId.");
            }

            try
            {
                var media = await _albumMediaService.GetAlbumMediaByIdAsync(mediaId);
                if (media == null)
                {
                    _logger.LogWarning("No AlbumMedia found with Id {MediaId}.", mediaId);
                    return Json(new { success = false, message = "Media not found." });
                }

                await _albumMediaService.DeleteAlbumMediaAsync(mediaId);

                _logger.LogInformation("AlbumMedia {MediaId} deleted successfully from Album {AlbumId}.", mediaId, media.AlbumId);
                return Json(new { success = true, message = "Media deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting AlbumMedia {MediaId}.", mediaId);
                return Json(new { success = false, message = "An error occurred while deleting media." });
            }
        }

    }
}
