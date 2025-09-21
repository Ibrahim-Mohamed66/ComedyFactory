using Admin.DataTable;
using Admin.ViewModels;
using Application.DTOS;
using Application.IServices;
using Application.Services;
using AutoMapper;
using Domain.Enums;
using Domain.StaticData;
using IoC;
using IOC.Resources;
using JqueryDataTables.ServerSide.AspNetCoreWeb.ActionResults;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Admin.Controllers
{

    [Authorize(Roles =RoleNames.Admin)]
    public class BlockController : Controller
    {
        private readonly ILogger<BlockController> _logger;
        private readonly IMapper _mapper;
        private readonly IBlockService _blockService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public BlockController(
            ILogger<BlockController> logger,
            IMapper mapper,
            IBlockService blockService,
            IStringLocalizer<SharedResource> localizer)
        {
            _logger = logger;
            _mapper = mapper;
            _blockService = blockService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(BlockType blockType)
        {
            SetViewData(blockType);

            var block = await _blockService.GetBlockByTypeAsync(blockType);
            if (block is not null)
                return RedirectToAction(nameof(Update), new { blockType, id = block.Id });

            return RedirectToAction(nameof(Create), new { blockType });
        }

        [HttpGet]
        public IActionResult Create(BlockType blockType)
        {
            SetViewData(blockType);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlockType blockType, BlockViewModel model)
        {
            SetViewData(blockType);
            model.BlockType = blockType;

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var blockDto = _mapper.Map<BlockDto>(model);
                var result = await _blockService.AddBlockAsync(blockDto);

                if (result is not null)
                    return RedirectToAction(nameof(Update), new { blockType, id = result.Id });

                ModelState.AddModelError("", _localizer["Failed to create block."]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating block of type {BlockType}", blockType);
                ModelState.AddModelError("", _localizer["An unexpected error occurred."]);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(BlockType blockType, int id)
        {
            if (id <= 0) return NotFound();

            SetViewData(blockType);

            var block = await _blockService.GetBlockByIdAsync(id);
            if (block is null) return NotFound();

            var model = _mapper.Map<BlockViewModel>(block);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BlockType blockType, int id, BlockViewModel model)
        {
            if (id != model.Id) return NotFound();

            SetViewData(blockType);

            if (!ModelState.IsValid) return View(model);

            try
            {
                var blockDto = _mapper.Map<BlockDto>(model);
                var result = await _blockService.UpdateBlockAsync(id, blockDto);

                if (result is not null)
                    return RedirectToAction(nameof(Update), new { blockType, id = result.Id });

                ModelState.AddModelError("", _localizer["Failed to update block."]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating block {BlockId} of type {BlockType}", id, blockType);
                ModelState.AddModelError("", _localizer["An unexpected error occurred."]);
            }

            return View(model);
        }

        
        // Centralize ViewData setup
        private void SetViewData(BlockType blockType)
        {
            ViewData["Main"] = _localizer[blockType.DisplayName()];
            ViewData["BlockType"] = blockType;
        }
    }
}
