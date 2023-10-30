using BaseHA.Application.ModelDto.DTO;
using BaseHA.Core.Extensions;
using BaseHA.Domain.Entity;
using BaseHA.Models.SearchModel;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using BaseHA.Core.Extensions;
using BaseHA.Core.IRepositories;
using BaseHA.Core.Base;

namespace BaseHA.Application.Serivce
{
    public interface IAnswerSerive
    {
        Task<bool> InsertAsync(Answer entity);

        Task<bool> InsertAsync(IEnumerable<Answer> entities);

        Task<bool> UpdateAsync(Answer entity);

        Task<bool> DeletesAsync(IEnumerable<string> ids);

        Task<PagedList<Answer>> GetAsync(AnswerSearchModel ctx);

        Task<Answer> GetByIdAsync(string id, bool tracking = false);

        Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active);
        
        Task<IList<SelectListItem>> GetSelectListItem();
    }



    public class AnswerService : IAnswerSerive
    {
        private readonly IRepositoryEF<Answer> _generic;

        public AnswerService()
        {
            _generic = EngineContext.Current.Resolve<IRepositoryEF<Answer>>(DataConnectionHelper.ConnectionStringNames.Warehouse);

        }

        public async Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));
            var list = await _generic.WhereTracking(x => ids.Contains(x.Id) && !x.OnDelete).ToListAsync();
            if (list == null)
                return false;
            list.ForEach(x => x.Inactive = active);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> DeletesAsync(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));
            await _generic.DeteleSoftDelete(ids);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<PagedList<Answer>> GetAsync(AnswerSearchModel ctx)
        {
            var l = from i in _generic.Table where i.OnDelete == false orderby i.AnswerVn ascending select i;

            if (!string.IsNullOrEmpty(ctx.Keywords))
                l = from aa in l
                    where aa.AnswerVn.Contains(ctx.Keywords)
                    orderby aa.AnswerVn ascending
                    select aa;

            if (!string.IsNullOrEmpty(ctx.CategoryId))
            {
                // lấy các con của id và tìm kiếm

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("WITH cte (Id, Name, ParentId) AS (");
                queryBuilder.Append("    SELECT");
                queryBuilder.Append("        wh.Id,");
                queryBuilder.Append("        wh.NameCategory as Name,");
                queryBuilder.Append("        wh.ParentId");
                queryBuilder.Append("    FROM");
                queryBuilder.Append("        Categories wh");
                queryBuilder.Append("    WHERE");
                queryBuilder.Append("        wh.ParentId = '" + ctx.CategoryId + "'");
                queryBuilder.Append("    UNION ALL");
                queryBuilder.Append("    SELECT");
                queryBuilder.Append("        p.Id,");
                queryBuilder.Append("        p.NameCategory as Name,");
                queryBuilder.Append("        p.ParentId");
                queryBuilder.Append("    FROM");
                queryBuilder.Append("        Categories p");
                queryBuilder.Append("    INNER JOIN");
                queryBuilder.Append("        cte");
                queryBuilder.Append("    ON");
                queryBuilder.Append("        p.ParentId = cte.Id");
                queryBuilder.Append(" ) ");
                queryBuilder.Append(" SELECT ");
                queryBuilder.Append("    Id");
                queryBuilder.Append(" FROM ");
                queryBuilder.Append("    cte");
                queryBuilder.Append(" GROUP BY ");
                queryBuilder.Append("    Id, Name, ParentId;");

                var departmentIds = (await _generic.QueryAsync<string>(queryBuilder.ToString())).ToList();

                departmentIds.Add(ctx.CategoryId);
                if (departmentIds != null && departmentIds.Any())
                    l = from aa in l where departmentIds.Contains(aa.CategoryId) orderby aa.AnswerVn ascending select aa;
            }
            PagedList<Answer> res = new PagedList<Answer>();
            await res.Result(ctx.PageSize, (ctx.PageIndex - 1) * ctx.PageSize, l);
            return res;
        }

        public async Task<Answer> GetByIdAsync(string id, bool tracking = false)
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
                        Text = $"{i.AnswerVn}",
                        Value = i.Id
                    };
            return await q.ToListAsync();
        }

        public async Task<bool> InsertAsync(Answer entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await _generic.AddAsync(entity);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> InsertAsync(IEnumerable<Answer> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            _generic.Update(entities);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Answer entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _generic.Update(entity);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

    }

}
