using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.IRepositories
{
    public interface IBlockRepository
    {
        Task<bool> AddBlockAsync(Block block);
        Task<bool> UpdateBlockAsync(Block block);
        Task<bool> DeleteBlockAsync(int blockId);
        Task<Block?> GetBlockByIdAsync(int blockId);
        Task<Block> GetBlockByTypeAsync(BlockType blockType);

        public Task<Block> GetFirstOrDefaultAsync(Expression<Func<Block, bool>>? filter = null);
        public Task<bool> AnyAsync();
    }
}
