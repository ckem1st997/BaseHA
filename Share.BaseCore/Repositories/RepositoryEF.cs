using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Share.BaseCore.IRepositories;
using Nest;
using System.Data.Common;
using Dapper;
using Confluent.Kafka;
using Microsoft.Data.SqlClient;
using StackExchange.Redis;
using static Dapper.SqlMapper;
using Share.BaseCore.Base;
using Share.BaseCore.DiagnosticListener;
using System.Reflection;

namespace Share.BaseCore.Repositories
{
    public class RepositoryEF<T> : IRepositoryEF<T> where T : class
    {

        //  _rep = EngineContext.Current.Resolve<IRepositoryEF<Domain.Entity.WareHouse>>(DataConnectionHelper.ConnectionStringNames.Warehouse);

        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IQueryable<T> _query;
        private readonly string _connectionstring;


        public DbContext UnitOfWork
        {
            get { return _context; }
        }
        //public bool HasIdProperty()
        //{
        //    Type type = typeof(T);
        //    PropertyInfo idProperty = type.GetProperty("Id");
        //    PropertyInfo idPropertyDelete = type.GetProperty("Ondelete");

        //    return idProperty != null && idPropertyDelete != null;
        //}
        public IQueryable<T> GetQueryable(bool tracking = true)
        {
            return tracking ? _query : _query.AsNoTracking();
        }
        public IQueryable<T> Table => _query.AsNoTracking();

        public RepositoryEF(DbContext context)
        {
            //if (!HasIdProperty())
            //    throw new BaseException("Entity not BaseEnity");
            // System.Diagnostics.DiagnosticListener.AllListeners.Subscribe(new DiagnosticObserver());
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
            _query = _dbSet.AsQueryable();
            _connectionstring = _context.Database.IsInMemory() ? "" : _context.Database.GetConnectionString() ?? throw new ArgumentNullException("GetConnectionString is null !");
        }



        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
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

        public virtual async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
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

    }

}