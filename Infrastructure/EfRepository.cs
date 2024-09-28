using System.Linq.Expressions;
using Core.Enities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

   public class EfRepository<T> : IAsyncRepository<T> where T : class
    {
        private readonly TranslationManagementDbContext _dbContext;
        public EfRepository(TranslationManagementDbContext dbContext)
        {
             _dbContext = dbContext;
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public virtual async Task<T?> GetByIdAsync(string id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<T?> GetByIdAsync(ISpecification<T> spec)
        {
             return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        
        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<(IReadOnlyList<T?> list,int totalCount)> ListAsync(ISpecification<T> spec)
        {
            return (await ApplySpecification(spec).ToListAsync(), await _dbContext.Set<T>().CountAsync());
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        } 
        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        } 
        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        } 
        private IQueryable<T?> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        } 
        
         
    }