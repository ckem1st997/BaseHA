using BaseHA.Domain.Entity;
using BaseHA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.EntityFrameworkCore;
using Nest;
using NuGet.Packaging.Signing;
using Share.BaseCore.Base;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;
using System.Threading.Tasks;

namespace BaseHA.Serivce
{
    public interface IWareHouseService
    {
        Task<bool> InsertAsync(WareHouse entity);

        Task<bool> InsertWHAsync(IEnumerable<WareHouse> entities);

        Task<bool> UpdateAsync(WareHouse entity);

        Task<bool> DeletesAsync(IEnumerable<string> ids);

        Task<PagedList<WareHouse>> GetAsync(WareHouseSearchModel ctx);

        Task<WareHouse> GetByIdAsync(string id);

        Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active);
    }


    public class WareHouseService : IWareHouseService
    {
        private readonly IRepositoryEF<WareHouse> _generic;

        public WareHouseService()
        {
            _generic = EngineContext.Current.Resolve<IRepositoryEF<WareHouse>>(DataConnectionHelper.ConnectionStringNames.Warehouse);

        }

        public async Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var list = await _generic.WhereTracking(x => ids.Contains(x.Id) && !x.OnDelete).ToListAsync();

            if (list == null)
                return false;

            list.ForEach(x => x.Inactive = active);

            //  _generic.Update(list);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> DeletesAsync(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var list = await _generic.Table.Where(x => ids.Contains(x.Id) && x.OnDelete == false).ToListAsync();

            if (list == null)
                throw new ArgumentNullException("list is null !");

            list.ForEach(x => x.OnDelete = true);
            //  _generic.Update(list);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<PagedList<WareHouse>> GetAsync(WareHouseSearchModel ctx)
        {
            var l = from i in _generic.Table where i.OnDelete == false select i;
            if (!string.IsNullOrEmpty(ctx.Keywords))
                l = from aa in l where aa.Name.Contains(ctx.Keywords) || aa.Code.Contains(ctx.Keywords) select aa;

            var data = await l.Skip((ctx.PageIndex - 1) * ctx.PageSize).Take(ctx.PageSize).ToListAsync();
            PagedList<WareHouse> res = new PagedList<WareHouse>();
            await res.Result(ctx.PageSize, (ctx.PageIndex - 1) * ctx.PageSize, l);
            return res;
        }

        public async Task<WareHouse> GetByIdAsync(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id is null !");
            return await _generic.Table.FirstOrDefaultAsync(x => id.Equals(x.Id) && !x.OnDelete);

        }

        public async Task<bool> InsertAsync(WareHouse entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await _generic.AddAsync(entity);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> InsertWHAsync(IEnumerable<WareHouse> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            _generic.Update(entities);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> UpdateAsync(WareHouse entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _generic.Update(entity);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }
    }
}
