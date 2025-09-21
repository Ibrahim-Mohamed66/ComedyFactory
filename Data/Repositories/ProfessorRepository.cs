using Data.Context;
using Data.Repositories.IRepositories;
using DocumentFormat.OpenXml.Bibliography;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    internal class ProfessorRepository:IProfessorRepository
    {
        private readonly WriteDbContext _context;
        private readonly ReadDbContext _read;
        public ProfessorRepository(WriteDbContext context, ReadDbContext readContext)
        {
            _context = context;
            _read = readContext;
        }
        public async Task<IEnumerable<Professor>> GetAllProfessorsAsync(
            Expression<Func<Professor, bool>>? filter = null,
            Func<IQueryable<Professor>, IOrderedQueryable<Professor>>? orderBy = null,
            Func<IQueryable<Professor>, IIncludableQueryable<Professor, object>>? include = null)
        {
            IQueryable<Professor> query = _read.Professors.Where(c => !c.Deleted);

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Professor?> GetProfessorByIdAsync(int id)
        {
            return await _read.Professors
                            .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

        }

        public async Task<bool> AddProfessorAsync(Professor Professor)
        {
            await _context.Professors.AddAsync(Professor);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateProfessorAsync(Professor Professor)
        {
            _context.Professors.Update(Professor);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<Professor?> GetProfessorForUpdateAsync(int id)
        {
            return await _context.Professors.FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
        }
        public async Task<bool> DeleteProfessorAsync(int id)
        {
            var entity = await _context.Professors.FindAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                _context.Professors.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<int> GetCountAsync(Expression<Func<Professor, bool>>? filter = null)
        {
            IQueryable<Professor> query = _read.Professors;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }
    }
}
