using BaseHA.Domain.Entity;
using BaseHA.Models.SearchModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Share.BaseCore.Base;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;

namespace BaseHA.Application.Serivce
{
    public interface ICategoryService
    {
        Task<bool> InsertAsync(Category entities);

        Task<bool> InsertWHAsync(IEnumerable<Category> entities);

        Task<bool> UpdateAsync(Category entity);

        Task<bool> DeletesAsync(IEnumerable<string> ids);

        Task<PagedList<Category>> GetAsync(CategorySearchModel ctx);

        Task<Category> GetByIdAsync(string id, bool tracking = false);

        Task<Category> GetByENcode(string codeEn);

        Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active);
        //Task<IList<SelectListItem>> GetSelectListItem();
    }
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryEF<Category> _category;
        public CategoryService()
        {
            _category = EngineContext.Current.Resolve<IRepositoryEF<Category>>(DataConnectionHelper.ConnectionStringNames.Warehouse);
        }

        public async Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active)
        {
            if(ids == null)
                throw new ArgumentNullException(nameof(ids));
             var list= await _category.WhereTracking(x => ids.Contains(x.Id) && !x.OnDelete).ToListAsync();
            if(list == null)
            {
                return false;
            }
            list.ForEach(x => x.Inactive = active);
            return await _category.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> DeletesAsync(IEnumerable<string> ids)
        {
            if(ids == null)
            throw new NotImplementedException();

            await _category.DeteleSoftDelete(ids);
            return await _category.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<PagedList<Category>> GetAsync(CategorySearchModel ctx)
        {
            var list = from cate in _category.Table where cate.OnDelete==false select cate;
            if (!string.IsNullOrEmpty(ctx.Keywords))
            {
                list = from c in list where c.NameCategory.Contains(ctx.Keywords) || c.IntentCodeEn.Contains(ctx.Keywords)
                                            || c.IntentCodeVn.Contains(ctx.Keywords) select c;
            }
            PagedList<Category> res = new PagedList<Category>();
            await res.Result(ctx.PageSize, (ctx.PageIndex - 1) * ctx.PageSize, list);
   
            return res;
        }

        public async Task<Category> GetByENcode(string codeEn)
        {
            if (codeEn == null)
                throw new NotImplementedException();

            var record = from i in _category.Table
                         where i.IntentCodeEn == codeEn && !i.OnDelete
                         select i;

            return await record.FirstAsync<Category>();
        }

        public async Task<Category> GetByIdAsync(string id, bool tracking = false)
        {
            if(id == null)
                throw new NotImplementedException();
            return await _category.GetByIdsync(id, Tracking: tracking);
        }

        public async Task<bool> InsertAsync(Category entities)
        {
            if(entities == null)
                throw new ArgumentNullException(nameof(entities));
            await _category.AddAsync(entities);
            return await _category.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> InsertWHAsync(IEnumerable<Category> entities)
        {
            if(entities == null)
                throw new ArgumentNullException(nameof(entities));
            _category.Update(entities);
            return await _category.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Category entity)
        {
            if(entity == null)
                throw new ArgumentNullException(nameof(entity));
            _category.Update(entity);
            return await _category.SaveChangesConfigureAwaitAsync() > 0;
        }
    }
}
