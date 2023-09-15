using BaseHA.Domain.Entity;
using BaseHA.Models.SearchModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.EntityFrameworkCore;
using Nest;
using NuGet.Packaging.Signing;
using Share.BaseCore.Base;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;
using System.Threading.Tasks;

namespace BaseHA.Application.Serivce
{
    public interface IBeginningWareHouseService
    {
        Task<bool> InsertAsync(BeginningWareHouse entity);

        Task<bool> InsertWHAsync(IEnumerable<BeginningWareHouse> entities);

        Task<bool> UpdateAsync(BeginningWareHouse entity);

        Task<bool> DeletesAsync(IEnumerable<string> ids);

        Task<PagedList<BeginningWareHouse>> GetAsync(BeginningWareHouseModel ctx);

        Task<BeginningWareHouse> GetByIdAsync(string id, bool tracking = false);

        Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active);
        
        Task<IList<SelectListItem>> GetSelectListItem();
        Task<IList<SelectListItem>> GetSelectListWareHouse();
        Task<IList<SelectListItem>> GetSelectListUnit();
    }


    public class BeginningWareHouseService : IBeginningWareHouseService
    {
        private readonly IRepositoryEF<BeginningWareHouse> _generic;

        private readonly IRepositoryEF<WareHouse> _tableWareHouse;
        private readonly IRepositoryEF<WareHouseItem> _tableItem;
        private readonly IRepositoryEF<Unit> _tableUnit;

        public BeginningWareHouseService()
        {
            _generic = EngineContext.Current.Resolve<IRepositoryEF<BeginningWareHouse>>(DataConnectionHelper.ConnectionStringNames.Warehouse);
            _tableWareHouse = EngineContext.Current.Resolve<IRepositoryEF<WareHouse>>(DataConnectionHelper.ConnectionStringNames.Warehouse);
            _tableItem = EngineContext.Current.Resolve<IRepositoryEF<WareHouseItem>>(DataConnectionHelper.ConnectionStringNames.Warehouse);
            _tableUnit = EngineContext.Current.Resolve<IRepositoryEF<Unit>>(DataConnectionHelper.ConnectionStringNames.Warehouse);
        }

        public async Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var list = await _generic.WhereTracking(x => ids.Contains(x.Id) && !x.OnDelete).ToListAsync();

            if (list == null)
                return false;

            //list.ForEach(x => x.Inactive = active);

            //  _generic.Update(list);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> DeletesAsync(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            //var list = await _generic.GetQueryable().Where(x => ids.Contains(x.Id) && x.OnDelete == false).ToListAsync();

            //if (list == null)
            //    throw new ArgumentNullException("list is null !");

            //list.ForEach(x => x.OnDelete = true);
            //  _generic.Update(list);
            await _generic.DeteleSoftDelete(ids);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<PagedList<BeginningWareHouse>> GetAsync(BeginningWareHouseModel ctx)
        {
            var l = from i in _generic.Table where i.OnDelete == false select i;
            if (!string.IsNullOrEmpty(ctx.Keywords))
                l = from aa in l where 
                    aa.WareHouseId.Contains(ctx.Keywords) || aa.ItemId.Contains(ctx.Keywords) || aa.UnitId.Contains(ctx.Keywords)
                    || aa.UnitName.Contains(ctx.Keywords) || aa.Quantity.Equals(ctx.Keywords) 
                    || aa.CreatedDate.Equals(ctx.Keywords) || aa.CreatedBy.Equals(ctx.Keywords)
                    || aa.ModifiedDate.Equals(ctx.Keywords) || aa.ModifiedBy.Equals(ctx.Keywords)
                    select aa;

            PagedList<BeginningWareHouse> res = new PagedList<BeginningWareHouse>();
            await res.Result(ctx.PageSize, (ctx.PageIndex - 1) * ctx.PageSize, l);
            return res;
        }

        public async Task<BeginningWareHouse> GetByIdAsync(string id, bool tracking = false)
        {
            if (id == null)
                throw new ArgumentNullException("id is null !");
            return await _generic.GetByIdsync(id, Tracking: tracking);

        }
        public async Task<IList<SelectListItem>> GetSelectListWareHouse()
        {
            var q = from i in _tableWareHouse.Table
                    where i.OnDelete == false
                    select new SelectListItem
                    {
                        Text = $"{i.Id} - {i.Name}",
                        Value = i.Id
                    };
            return await q.ToListAsync();
        }
        public async Task<IList<SelectListItem>> GetSelectListUnit()
        {
            var q = from i in _tableUnit.Table
                    where !i.OnDelete
                    select new SelectListItem
                    {
                        Text = $"{i.Id}-{i.UnitName}",
                        Value = i.Id
                    };
            return await q.ToListAsync();
        }
        public async Task<IList<SelectListItem>> GetSelectListItem()
        {
            var q = from i in _tableItem.Table
                    where !i.OnDelete
                    select new SelectListItem
                    {
                        Text = $"{i.Id}-{i.Name}",
                        Value = i.Id
                    };
            return await q.ToListAsync();
        }

        public async Task<bool> InsertAsync(BeginningWareHouse entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await _generic.AddAsync(entity);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> InsertWHAsync(IEnumerable<BeginningWareHouse> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            _generic.Update(entities);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> UpdateAsync(BeginningWareHouse entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _generic.Update(entity);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }
    }
}
