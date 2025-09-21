using Data.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Data.Context;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public class BlockRepository : IBlockRepository
    {
        private readonly WriteDbContext _context;
        public BlockRepository(WriteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddBlockAsync(Block block)
        {
            await _context.Blocks.AddAsync(block);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AnyAsync()
        {
            return await _context.Blocks.AnyAsync();
        }

        public async Task<bool> DeleteBlockAsync(int blockId)
        {
            var block = await _context.Blocks.FindAsync(blockId);
            if(block is not null)
            {
                block.Deleted = true;

                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Block?> GetBlockByIdAsync(int blockId)
        {
            return await _context.Blocks.FindAsync(blockId);
        }

        public async Task<Block> GetBlockByTypeAsync(BlockType blockType)
        {
            return await _context.Blocks.FirstOrDefaultAsync(b=> b.BlockType == blockType && !b.Deleted);
        }

        public async Task<Block> GetFirstOrDefaultAsync(Expression<Func<Block, bool>>? filter = null)
        {
            IQueryable<Block> query = _context.Blocks;
            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateBlockAsync(Block block)
        {
            block.UpdatedOnUtc = DateTime.UtcNow;
            _context.Blocks.Update(block);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
