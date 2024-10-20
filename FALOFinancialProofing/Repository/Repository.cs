﻿using FALOFinancialProofing.Core;
using FALOFinancialProofing.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FALOFinancialProofing.Repository
{
    public class Repository<T, TPrimaryKey> : IRepository<T, TPrimaryKey> where T : Entity<TPrimaryKey>
    {
        public FALOFinancialProofingDbContext _dbContext;

        public Repository(FALOFinancialProofingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
                var rowCount = await _dbContext.SaveChangesAsync();
                return rowCount > 0;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(TPrimaryKey id)
        {
            var entity = await Get(id);
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
                var rowCount = await _dbContext.SaveChangesAsync();
                return rowCount > 0;
            }

            return false;
        }
        public async Task<T> Get(TPrimaryKey id)
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(p => p.Id.Equals(id));
        }
        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(filter);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            var query = _dbContext.Set<T>().AsNoTracking();
            query = filter != null ? query.Where(filter) : query;
            return query;
        }

        public IQueryable<T> GetAllIncludeDeleted(Expression<Func<T, bool>> filter = null)
        {
            var query = filter != null ? _dbContext.Set<T>().Where(filter) : _dbContext.Set<T>();
            return query;
        }

        public async Task<T> GetFirstItem(Expression<Func<T, bool>> filter)
        {
            var query = _dbContext.Set<T>();
            return await query.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<T> InsertAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            var rowCount = await _dbContext.SaveChangesAsync();

            return rowCount > 0 ? entity : null;
        }

        public async Task<bool> InsertManyAsync(IEnumerable<T> entities)
        {
            entities.ToList();

            _dbContext.Set<T>().AddRange(entities);
            var rowCount = await _dbContext.SaveChangesAsync();
            return rowCount > 0;
        }

        //public async Task<bool> UpdateAsync(T entity)
        //{
        //    var dbEntity = await _dbContext.Set<T>().FindAsync(entity.Id);
        //    if (dbEntity == null)
        //        return false;

        //    _dbContext.Entry(dbEntity).CurrentValues.SetValues(entity);
        //    var rowCount = await _dbContext.SaveChangesAsync();
        //    return rowCount > 0;
        //}

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            var rowCount = await _dbContext.SaveChangesAsync();
            return rowCount > 0;
        }
    }
}