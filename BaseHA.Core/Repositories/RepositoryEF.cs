﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Nest;
using System.Data.Common;
using Dapper;
using Confluent.Kafka;
using Microsoft.Data.SqlClient;
using StackExchange.Redis;
using static Dapper.SqlMapper;
using BaseHA.Core.Base;
using BaseHA.Core.DiagnosticListener;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using BaseHA.Core.Base;
using BaseHA.Core.IRepositories;

namespace BaseHA.Core.Repositories
{
    public class RepositoryEF<T> : IRepositoryEF<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IQueryable<T> _query;
        private readonly string _connectionstring;
        public IQueryable<T> GetQueryable(bool tracking = true)
        {
            return tracking ? _query : _query.AsNoTracking();
        }
        public IQueryable<T> Table => _query.AsNoTracking();

        public RepositoryEF(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
            _query = _dbSet.AsQueryable();
            _connectionstring = _context.Database.IsInMemory() ? "" : _context.Database.GetConnectionString() ?? throw new ArgumentNullException("GetConnectionString is null !");
        }



        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {

            cancellationToken.ThrowIfCancellationRequested();
            // ThrowIfDisposed();
            if (entity is null)
                throw new BaseException(nameof(entity));
            return (await _dbSet.AddAsync(entity)).Entity;
        }

        public virtual void Delete(T entity)
        {
            if (entity is null)
                throw new BaseException(nameof(entity));
            _dbSet.Remove(entity);
        }


        public virtual void Update(T entity)
        {
            if (entity is null)
                throw new BaseException(nameof(entity));
            _dbSet.Update(entity);
        }

        public virtual async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // ThrowIfDisposed();
            if (entity == null)
            {
                throw new BaseException(nameof(entity));
            }
            _dbSet.Attach(entity);
            //   entity.ConcurrencyStamp = Guid.NewGuid().ToString();
            _dbSet.Update(entity);
            try
            {
                return await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return await Task.FromResult(false);
            }
        }



        public bool AutoSaveChanges { get; set; } = true;

        public DatabaseFacade Database => _context.Database;

        protected virtual async Task<bool> SaveChanges(CancellationToken cancellationToken)
        {
            if (AutoSaveChanges)
            {
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            return AutoSaveChanges;
        }

        public IEnumerable<T> GetList(Func<T, bool> filter)
        {
            IEnumerable<T> query = _dbSet;
            if (filter != null)
                query = query.Where(filter);
            return query.ToList();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _query.AsNoTracking().Where(predicate);
        }

        public async Task<IQueryable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(_query.AsNoTracking().Where(predicate));
        }


        public IQueryable<T> WhereTracking(Expression<Func<T, bool>> predicate)
        {
            return _query.Where(predicate);
        }

        public async Task<IQueryable<T>> WhereTrackingAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(_query.Where(predicate));
        }


        public async Task<int> SaveChangesConfigureAwaitAsync(bool configure = false, CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(configure);

        }

        public async Task AddAsync(IEnumerable<T> entity, CancellationToken cancellationToken = default)
        {
            if (entity is null)
                throw new BaseException(nameof(entity));
            await _dbSet.AddRangeAsync(entity);
        }

        public void Update(IEnumerable<T> entity)
        {
            if (entity is null)
                throw new BaseException(nameof(entity));
            _dbSet.UpdateRange(entity);
        }

        public void Delete(IEnumerable<T> entity)
        {
            if (entity is null)
                throw new BaseException(nameof(entity));
            _dbSet.RemoveRange(entity);
        }

        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_connectionstring);
        }

        public async Task<T1> QueryFirstOrDefaultAsync<T1>(string sp, DynamicParameters parms, System.Data.CommandType commandType = System.Data.CommandType.Text)
        {
            using var connection = new SqlConnection(_connectionstring);
            return await connection.QueryFirstOrDefaultAsync<T1>(sp, parms, commandType: commandType);
        }

        public T1 QueryFirst<T1>(string sp, DynamicParameters parms, System.Data.CommandType commandType = System.Data.CommandType.Text)
        {
            using var connection = new SqlConnection(_connectionstring);
            return connection.QueryFirst<T1>(sp, parms, commandType: commandType);
        }

        public async Task<IEnumerable<T1>> QueryAsync<T1>(string sp, DynamicParameters parms, System.Data.CommandType commandType = System.Data.CommandType.Text)
        {
            using var connection = new SqlConnection(_connectionstring);
            return await connection.QueryAsync<T1>(sp, parms, commandType: commandType);
        }

        public IEnumerable<T1> Query<T1>(string sp, DynamicParameters parms, System.Data.CommandType commandType = System.Data.CommandType.Text)
        {
            using var connection = new SqlConnection(_connectionstring);
            return connection.Query<T1>(sp, parms, commandType: commandType);
        }

        public async Task<GridReader> QueryMultipleAsync(string sp, DynamicParameters parms, System.Data.CommandType commandType = System.Data.CommandType.Text)
        {
            using var connection = new SqlConnection(_connectionstring);
            return await connection.QueryMultipleAsync(sp, param: parms, commandType: commandType);
        }


        public GridReader QueryMultiple(string sp, DynamicParameters parms, System.Data.CommandType commandType = System.Data.CommandType.Text)
        {
            using var connection = new SqlConnection(_connectionstring);
            return connection.QueryMultiple(sp, param: parms, commandType: commandType);
        }



        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<T>> DeteleSoftDelete(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            if (!ids.Any())
                throw new BaseException("Danh sách mã xoá rỗng !");
            var list = await _query.Where(x => ids.Contains(x.Id) && !x.OnDelete).ToListAsync();
            if (!list.Any())
                throw new BaseException("Danh sách cần xoá không tồn tại !");
            list.ForEach(x =>
           {
               x.OnDelete = true;
           });
            return list;
        }

        public async Task<T> DeteleSoftDelete(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
                throw new BaseException("Mã xoá rỗng !");
            var entity = await _query.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (entity == null)
                throw new BaseException("Danh sách cần xoá không tồn tại !");
            entity.OnDelete = true;
            return entity;
        }

        public async Task<T?> GetByIdsync(string id, CancellationToken cancellationToken = default, bool Tracking = false)
        {
            if (string.IsNullOrEmpty(id))
                throw new BaseException("Chưa nhập mã định danh !");
            if (!Tracking)
                return await _query.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            return await _query.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

    }

}