using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Data.Repositories;
using Data.Repositories.IRepositories;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BlockService:IBlockService
    {
        private readonly IBlockRepository _blockRepository;
        private readonly IMapper _mapper;
        public BlockService(IBlockRepository blockRepository, IMapper mapper)
        {
            _blockRepository = blockRepository;
            _mapper = mapper;
        }

        public async Task<BlockDto> AddBlockAsync(BlockDto block)
        {
            var entity = _mapper.Map<Block>(block);
            var result = await  _blockRepository.AddBlockAsync(entity);
            if (!result)
            {
                throw new Exception("Failed to add block");
            }
            return _mapper.Map<BlockDto>(entity);
        }

        public async Task<bool> AnyAsync()
        {
            return await _blockRepository.AnyAsync();
        }

        public async Task<bool> DeleteBlockAsync(int blockId)
        {
            var result = await _blockRepository.DeleteBlockAsync(blockId);
            if (!result)
            {
                throw new Exception("Failed to delete block");
            }
            return true;
        }

        public async Task<BlockDto?> GetBlockByIdAsync(int blockId)
        {
           var entity =  await _blockRepository.GetBlockByIdAsync(blockId);
           return entity == null ? null : _mapper.Map<BlockDto>(entity);

        }

        public async Task<BlockDto?> GetBlockByTypeAsync(BlockType blockType)
        {
           var block =  await _blockRepository.GetBlockByTypeAsync(blockType);
            return block == null ? null : _mapper.Map<BlockDto>(block);

        }

        public async Task<BlockDto> UpdateBlockAsync(int blockId, BlockDto block)
        {
            var entity = await _blockRepository.GetBlockByIdAsync(blockId);
            if (entity == null) return null;

            _mapper.Map(block, entity);
            entity.UpdatedOnUtc = DateTime.UtcNow;

            await _blockRepository.UpdateBlockAsync(entity);
            return _mapper.Map<BlockDto>(entity);
        }
    }
}
