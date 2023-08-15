using BaseHA.Domain.Entity;
using BaseHA.Models.SearchModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Share.BaseCore.Base;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;

namespace BaseHA.Application.Serivce
{
    public interface IVendorService
    {
        Task<bool> InsertAsync(Vendor entity);

        Task<bool> InsertWHAsync(IEnumerable<Vendor> entities);

        Task<bool> UpdateAsync(Vendor entity);

        Task<bool> DeletesAsync(IEnumerable<string> ids);

        Task<PagedList<Vendor>> GetAsync(VendorSearchModel ctx);

        Task<Vendor> GetByIdAsync(string id, bool tracking = false);

        Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active);
        
        Task<IList<SelectListItem>> GetSelectListItem();
    }

    public class VendorService : IVendorService
    {
        private readonly IRepositoryEF<Vendor> _generic;

        public VendorService()
        {
            _generic = EngineContext.Current.Resolve<IRepositoryEF<Vendor>>(DataConnectionHelper.ConnectionStringNames.Warehouse);

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

            //var list = await _generic.GetQueryable().Where(x => ids.Contains(x.Id) && x.OnDelete == false).ToListAsync();

            //if (list == null)
            //    throw new ArgumentNullException("list is null !");

            //list.ForEach(x => x.OnDelete = true);
            //  _generic.Update(list);
            await _generic.DeteleSoftDelete(ids);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<PagedList<Vendor>> GetAsync(VendorSearchModel ctx)
        {
            var l = from i in _generic.Table select i; // where i.OnDelete == false select i;
            if (!string.IsNullOrEmpty(ctx.Keywords))
                l = from aa in l where aa.Name.Contains(ctx.Keywords) || aa.Code.Contains(ctx.Keywords) || aa.Phone.Contains(ctx.Keywords) select aa;
            PagedList<Vendor> res = new PagedList<Vendor>();
            await res.Result(ctx.PageSize, (ctx.PageIndex - 1) * ctx.PageSize, l);
            return res;
        }

        public async Task<Vendor> GetByIdAsync(string id, bool tracking = false)
        {
            if (id == null)
                throw new ArgumentNullException("id is null !");
            return await _generic.GetByIdsync(id, Tracking: tracking);

        }

        public async Task<IList<SelectListItem>> GetSelectListItem()
        {
            var q = from i in _generic.Table
                    where !i.OnDelete
                    select new SelectListItem
                    {
                        Text = $"{i.Code}-{i.Name}",
                        Value = i.Id
                    };
            return await q.ToListAsync();
        }

        public async Task<bool> InsertAsync(Vendor entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await _generic.AddAsync(entity);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> InsertWHAsync(IEnumerable<Vendor> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            _generic.Update(entities);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Vendor entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            //_generic.Update(entity);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }
    }
}
