using BaseHA.Application.ModelDto;
using BaseHA.Domain.Entity;
using BaseHA.Models.SearchModel;
using Microsoft.AspNetCore.Mvc;
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
    public interface ICategoryTbService
    {
        Task<bool> InsertAsync(CategoryTb entity);

        Task<bool> InsertWHAsync(IEnumerable<CategoryTb> entities);

        Task<bool> UpdateAsync(CategoryTb entity);

        Task<bool> DeletesAsync(IEnumerable<string> ids);

        Task<PagedList<CategoryTb>> GetAsync(CategoryTbSearchModel ctx);

        Task<CategoryTb> GetByIdAsync(string id, bool tracking = false);

        Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active);

        //Task<IList<SelectListItem>> GetSelectListItem();
    }


    public class CategoryTbService : ICategoryTbService
    {
        private readonly IRepositoryEF<CategoryTb> _generic;

        public CategoryTbService()
        {
            _generic = EngineContext.Current.Resolve<IRepositoryEF<CategoryTb>>(DataConnectionHelper.ConnectionStringNames.CategoryTb);

        }

        public async Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var list = await _generic.WhereTracking(x => ids.Contains(x.Id) && !x.OnDelete).ToListAsync();

            if (list == null)
                return false;

            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> DeletesAsync(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

        
            await _generic.DeteleSoftDelete(ids);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<PagedList<CategoryTb>> GetAsync(CategoryTbSearchModel ctx)
        {
            var l = from i in _generic.Table select i;
            if (!string.IsNullOrEmpty(ctx.Keywords))
                l = from aa in l where aa.IntentCodeEn.Contains(ctx.Keywords) || aa.Category.Contains(ctx.Keywords) select aa;
            PagedList<CategoryTb> res = new PagedList<CategoryTb>();
            await res.Result(ctx.PageSize, (ctx.PageIndex - 1) * ctx.PageSize, l);
            return res;
        }

        public async Task<CategoryTb> GetByIdAsync(string id, bool tracking = false)
        {
            if (id == null)
                throw new ArgumentNullException("id is null !");
            return await _generic.GetByIdsync(id, Tracking: tracking);

        }
        
       /* public async Task<IList<SelectListItem>> GetSelectListItem()
        {
            var q = from i in _generic.Table
                    where !i.OnDelete
                    select new SelectListItem
                    {
                        Text = $"{i.Category}-{i.IntentCodeEn}",
                        Value = i.Id
                    };
            return await q.ToListAsync();
        }*/

        public async Task<bool> InsertAsync(CategoryTb entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            //entity.Id = Guid.NewGuid().ToString();
            await _generic.AddAsync(entity);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }
   


        public async Task<bool> InsertWHAsync(IEnumerable<CategoryTb> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            _generic.Update(entities);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> UpdateAsync(CategoryTb entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _generic.Update(entity);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }
    }
}
