using Microsoft.EntityFrameworkCore;
using NFC.Application.Interfaces;
using NFC.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Infrastructure.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _dbContext;

        private readonly DbSet<TEntity> _dbset;

        

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbset = dbContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking) => asNoTracking ?
            await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync()
            :await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(Guid id) => await _dbContext.Set<TEntity>().FindAsync(id);

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public async Task<TEntity?> GetByIdWithIncludesAsync(Guid id, params Expression<Func<TEntity, object>>[] includees)
        {
            IQueryable<TEntity> query = _dbset;
            foreach (var include in includees)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public async Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(params Expression<Func<TEntity, object>>[] includees)
        {
            IQueryable<TEntity> query = _dbset;
            foreach (var include in includees)
                query = query.Include(include);
            return await query.ToListAsync();
        }
    }
}
