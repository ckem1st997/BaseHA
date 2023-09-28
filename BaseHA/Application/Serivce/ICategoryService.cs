using BaseHA.Application.ModelDto.DTO;
using BaseHA.Domain.Entity;
using BaseHA.Models.SearchModel;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Share.BaseCore.Base;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;
using System.Data;
using System.Text;
using static Nest.JoinField;

namespace BaseHA.Application.Serivce
{
    public interface ICategoryService
    {
        Task<bool> InsertAsync(Category entity);

        Task<bool> InsertAsync(IEnumerable<Category> entities);

        Task<bool> UpdateAsync(Category entity);

        Task<bool> DeletesAsync(IEnumerable<string> ids);

        Task<PagedList<Category>> GetAsync(CategorySearchModel ctx);

        Task<Category> GetByIdAsync(string id, bool tracking = false);

        Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active);
        Task<IList<SelectListItem>> GetSelectListItem();
        Task<IList<CategoryTreeModel>> GetTree(int? expandLevel);

        public bool IsIntentEnDuplicate(string intentCodeEn);
        public bool IsIntentVnDuplicate(string intentCodeVn);
    }



    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryEF<Category> _generic;

        public CategoryService()
        {
            _generic = EngineContext.Current.Resolve<IRepositoryEF<Category>>(DataConnectionHelper.ConnectionStringNames.Warehouse);

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

        public  bool IsIntentEnDuplicate(string intentCodeEn)
        {
            bool isDuplicate = _generic.Table.Any(e => e.IntentCodeEn == intentCodeEn );
            return isDuplicate;
        }
        public bool IsIntentVnDuplicate(string intentCodeVn)
        {
            bool isDuplicate = _generic.Table.Any(e => e.IntentCodeEn == intentCodeVn);
            return isDuplicate;
        }

        public async Task<PagedList<Category>> GetAsync(CategorySearchModel ctx)
        {
            var l = from i in _generic.Table where i.OnDelete==false select i;

            if (!string.IsNullOrEmpty(ctx.Keywords))
                l = from aa in l
                    where aa.NameCategory.Contains(ctx.Keywords)
                    || aa.IntentCodeEn.Contains(ctx.Keywords)
                    || aa.IntentCodeVn.Contains(ctx.Keywords)
                    select aa;

            if (!string.IsNullOrEmpty(ctx.CategoryId))
            {
                // lấy các con của id và tìm kiếm

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("WITH cte (Id, Name, ParentId) AS (");
                queryBuilder.Append("    SELECT");
                queryBuilder.Append("        wh.Id,");
                queryBuilder.Append("        wh.NameCategory,");
                queryBuilder.Append("        wh.ParentId");
                queryBuilder.Append("    FROM");
                queryBuilder.Append("        Categories wh");
                queryBuilder.Append("    WHERE");
                queryBuilder.Append("        wh.ParentId = '" + ctx.CategoryId + "'");
                queryBuilder.Append("    UNION ALL");
                queryBuilder.Append("    SELECT");
                queryBuilder.Append("        p.Id,");
                queryBuilder.Append("        p.NameCategory,");
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
                    l = from aa in l where departmentIds.Contains(aa.Id) select aa;
            }
            PagedList<Category> res = new PagedList<Category>();
            await res.Result(ctx.PageSize, (ctx.PageIndex - 1) * ctx.PageSize, l);
            return res;
        }

        public async Task<Category> GetByIdAsync(string id, bool tracking = false)
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
                        Text = $"[{i.NameCategory}] - "+i.IntentCodeVn,
                        Value = i.Id
                    };
            return await q.ToListAsync();
        }

        public async Task<IList<CategoryTreeModel>> GetTree(int? expandLevel)
        {
            expandLevel = expandLevel ?? 1;
            var qq = new Queue<CategoryTreeModel>();
            var lstCheck = new List<CategoryTreeModel>();
            var result = new List<CategoryTreeModel>();
            string sql = "select * from Categories where Inactive =@active and OnDelete=0 ";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@active", 1);
            var getAll = await _generic.QueryAsync<Category>(sql, parameter, CommandType.Text);
           
            var organizationalUnitModels = getAll
                .Select(s => new CategoryTreeModel
                {
                    children = new List<CategoryTreeModel>(),
                    folder = false,
                    key = s.Id,
                    //title = s.IntentCodeVn,
                    title= s.Description,
                    tooltip = s.NameCategory,
                    ParentId = s.ParentId,
                    Name = "[" + s.NameCategory + "]-" + s.IntentCodeVn
                });

            var roots = organizationalUnitModels.Where(w => !w.ParentId.HasValue()).OrderBy(o => o.Name);

            foreach (var root in roots)
            {
                root.level = 1;
                root.expanded = !expandLevel.HasValue || root.level <= expandLevel.Value;
                root.folder = true;
                qq.Enqueue(root);
                lstCheck.Add(root);
                result.Add(root);
            }

            while (qq.Any())
            {
                var cur = qq.Dequeue();
                if (lstCheck.All(a => a.key != cur.key))
                    result.Add(cur);

                var childs = organizationalUnitModels
                    .Where(w => w.ParentId.HasValue() && w.ParentId.ToString() == cur.key)
                    .OrderBy(o => o.Name);

                if (!childs.Any())
                    continue;

                var childLevel = cur.level + 1;
                foreach (var child in childs)
                {
                    if (lstCheck.Any(a => a.key == child.key))
                        continue;

                    child.level = childLevel;
                    child.expanded = !expandLevel.HasValue || child.level <= expandLevel.Value;

                    qq.Enqueue(child);
                    lstCheck.Add(child);
                    cur.children.Add(child);
                }
            }

            return result;
        }

        public async Task<bool> InsertAsync(Category entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await _generic.AddAsync(entity);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> InsertAsync(IEnumerable<Category> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            _generic.Update(entities);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Category entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _generic.Update(entity);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        private List<Category> GetCategoryTreeModel(IEnumerable<Category> models)
        {
            var parents = models.Where(w => string.IsNullOrEmpty(w.ParentId)).OrderBy(o => o.NameCategory);

            var result = new List<Category>();
            var level = 0;
            foreach (var parent in parents)
            {
                result.Add(new Category
                {
                    Id = parent.Id,
                    ParentId = parent.ParentId,
                    NameCategory = parent.NameCategory,
                });
                GetChildCategoryTreeModel(ref models, parent.Id, ref result, level);
            }

            return result;
        }

        private void GetChildCategoryTreeModel(ref IEnumerable<Category> models, string parentId, ref List<Category> result, int level)
        {
            level++;
            var childs = models.Where(w => w.ParentId == parentId).OrderBy(o => o.NameCategory);

            if (childs.Any())
            {
                foreach (var child in childs)
                {
                    child.NameCategory = child.NameCategory;
                    result.Add(new Category()
                    {
                        Id = child.Id,
                        ParentId = child.ParentId,
                        NameCategory = GetTreeLevelString(level) + child.NameCategory,
                    });
                    GetChildCategoryTreeModel(ref models, child.Id, ref result, level);
                }
            }
        }

        public static string GetTreeLevelString(int level)
        {
            if (level <= 0)
                return "";

            var result = "";
            for (var i = 1; i <= level; i++)
            {
                result += "–";
            }

            return result;
        }

        
    }

}
