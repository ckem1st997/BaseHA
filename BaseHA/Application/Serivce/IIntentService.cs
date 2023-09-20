using BaseHA.Domain.Entity;
using BaseHA.Models.SearchModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Share.BaseCore.Base;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;

namespace BaseHA.Application.Serivce
{
    public interface IIntentService
    {
        Task<bool> InsertAsync(Intent entities);

        Task<bool> InsertWHAsync(IEnumerable<Intent> entities);

        Task<bool> UpdateAsync(Intent entity);

        Task<bool> DeletesAsync(IEnumerable<string> ids);

        Task<bool> DeleteAsyncID(string ids);

        Task<PagedList<Intent>> GetAsync(IntentSearchModel ctx);

        Task<Intent> GetByIdAsync(string id, bool tracking = false);

        Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active);
       
    }

    public class IntentService : IIntentService
    {
        private readonly IRepositoryEF<Intent> _intent;
        private readonly IRepositoryEF<Category> _category;
        public IntentService()
        {
            _intent = EngineContext.Current.Resolve<IRepositoryEF<Intent>>(DataConnectionHelper.ConnectionStringNames.Warehouse);
            _category= EngineContext.Current.Resolve<IRepositoryEF<Category>>(DataConnectionHelper.ConnectionStringNames.Warehouse);
        }

        public async Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));
            var list = await _intent.WhereTracking(x => ids.Contains(x.Id) && !x.OnDelete).ToListAsync();
            if (list == null)
            {
                return false;
            }
            list.ForEach(x => x.Inactive = active);
            return await _intent.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> DeleteAsyncID(string ids)
        {
            if (ids == null)
                throw new NotImplementedException();

            await _intent.DeteleSoftDelete(ids);
            return await _intent.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> DeletesAsync(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new NotImplementedException();

            await _intent.DeteleSoftDelete(ids);
            return await _intent.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<PagedList<Intent>> GetAsync(IntentSearchModel ctx)
        {
            var list = from cate in _intent.Table where !cate.OnDelete select cate;
            if (!string.IsNullOrEmpty(ctx.Keywords))
            {
                list = from c in list
                       where c.IntentCodeEn.Contains(ctx.Keywords) || c.IntentEn.Contains(ctx.Keywords)
                                            || c.IntentVn.Contains(ctx.Keywords)
                       select c;
            }
            PagedList<Intent> res = new PagedList<Intent>();
            await res.Result(ctx.PageSize, (ctx.PageIndex - 1) * ctx.PageSize, list);

            return res;
        }

        public async Task<Intent> GetByIdAsync(string id, bool tracking = false)
        {
            if (id == null)
                throw new NotImplementedException();
            return await _intent.GetByIdsync(id, Tracking: tracking);
        }


       /* public async Task<IList<SelectListItem>> GetSelectListItem()
        {
            var list = from i in _category.Table
                    where !i.OnDelete
                    select new SelectListItem
                    {
                        Text = $"{i.IntentCodeEn}",
                        Value = i.IntentCodeEn
                    };
            return await list.ToListAsync();
        }*/

        public async Task<bool> InsertAsync(Intent entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            await _intent.AddAsync(entities);
            return await _intent.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> InsertWHAsync(IEnumerable<Intent> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            _intent.Update(entities);
            return await _intent.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Intent entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _intent.Update(entity);
            return await _intent.SaveChangesConfigureAwaitAsync() > 0;
        }

    }
}
