using Application.DTOS;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices;

public interface IBlockService
{
    Task<BlockDto> AddBlockAsync(BlockDto block);
    Task<BlockDto> UpdateBlockAsync(int blockId,BlockDto block);
    Task<bool> DeleteBlockAsync(int blockId);
    Task<BlockDto?> GetBlockByIdAsync(int blockId);
    Task<bool> AnyAsync();
    Task<BlockDto?> GetBlockByTypeAsync(BlockType blockType);
}
