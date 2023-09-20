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

namespace BaseHA.Application.Serivce
{
    public interface ICategoryService
    {
        Task<bool> InsertAsync(Category entities);

        Task<bool> InsertWHAsync(IEnumerable<Category> entities);

        Task<bool> UpdateAsync(Category entity);

        Task<bool> DeletesAsync(IEnumerable<string> ids);

        Task<bool> DeleteAsyncID(string ids);

        Task<PagedList<Category>> GetAsync(CategorySearchModel ctx);

        Task<Category> GetByIdAsync(string id, bool tracking = false);

        //Task<Category> GetByENcode(string codeEn);

        Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active);


        Task<IList<CategoryTreeModel>> GetTree(int? expandLevel);
        Task<IList<Category>> GetSelect();
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
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));
            var list = await _category.WhereTracking(x => ids.Contains(x.Id) && !x.OnDelete).ToListAsync();
            if (list == null)
            {
                return false;
            }
            list.ForEach(x => x.Inactive = active);
            return await _category.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> DeletesAsync(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new NotImplementedException();

            await _category.DeteleSoftDelete(ids);
            return await _category.SaveChangesConfigureAwaitAsync() > 0;
        }
        public async Task<bool> DeleteAsyncID(string ids)
        {
            if (ids == null)
                throw new NotImplementedException();

            await _category.DeteleSoftDelete(ids);
            return await _category.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<PagedList<Category>> GetAsync(CategorySearchModel ctx)
        {
            var list = from cate in _category.Table where cate.OnDelete == false select cate;
            if (!string.IsNullOrEmpty(ctx.Keywords))
            {
                list = from c in list
                       where c.NameCategory.Contains(ctx.Keywords) || c.IntentCodeEn.Contains(ctx.Keywords)
                                            || c.IntentCodeVn.Contains(ctx.Keywords)
                       select c;
            }

            if (!string.IsNullOrEmpty(ctx.CategoryId))
            {
                // lấy các con của id và tìm kiếm

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("WITH cte (Id, NameCategory, ParentId) AS (");
                queryBuilder.Append("    SELECT");
                queryBuilder.Append("        c.Id,");
                queryBuilder.Append("        c.NameCategory ,");
                queryBuilder.Append("        c.ParentId");
                queryBuilder.Append("    FROM");
                queryBuilder.Append("        Categories c");
                queryBuilder.Append("    WHERE");
                queryBuilder.Append("        wh.ParentId = '" + ctx.CategoryId + "'");
                queryBuilder.Append("    UNION ALL");
                queryBuilder.Append("    SELECT");
                queryBuilder.Append("        p.Id,");
                queryBuilder.Append("        p.NameCategory,");
                queryBuilder.Append("        p.ParentId");
                queryBuilder.Append("    FROM");
                queryBuilder.Append("       Categories p");
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
                queryBuilder.Append("    Id, NameCategory, ParentId;");

                var departmentIds = (await _category.QueryAsync<string>(queryBuilder.ToString())).ToList();
                departmentIds.Add(ctx.CategoryId);
                if (departmentIds != null && departmentIds.Any())
                    list = from aa in list where departmentIds.Contains(aa.Id) select aa;
            }

            PagedList<Category> res = new PagedList<Category>();
            await res.Result(ctx.PageSize, (ctx.PageIndex - 1) * ctx.PageSize, list);

            return res;
        }

        /* public async Task<Category> GetByENcode(string codeEn)
         {
             if (codeEn == null)
                 throw new NotImplementedException();

             var record = from i in _category.Table
                          where i.IntentCodeEn == codeEn && !i.OnDelete
                          select i;

             return await record.FirstAsync<Category>();
         }*/

        public async Task<Category> GetByIdAsync(string id, bool tracking = false)
        {
            if (id == null)
                throw new NotImplementedException();
            return await _category.GetByIdsync(id, Tracking: tracking);
        }

        public async Task<bool> InsertAsync(Category entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            await _category.AddAsync(entities);
            return await _category.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> InsertWHAsync(IEnumerable<Category> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            _category.Update(entities);
            return await _category.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Category entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _category.Update(entity);
            return await _category.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<IList<CategoryTreeModel>> GetTree(int? expandLevel)
        {
            expandLevel = expandLevel ?? 1;
            var qq = new Queue<CategoryTreeModel>();
            var lstCheck = new List<CategoryTreeModel>();
            var result = new List<CategoryTreeModel>();
            string sql = "select Id, ParentId, NameCategory, IntentCodeEN, IntentCodeVN, Description from Categories where Inactive =@active and OnDelete=0 ";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@active", 1);
            var getAll = await _category.QueryAsync<CategotyDTO>(sql, parameter, CommandType.Text);
            var organizationalUnitModels = getAll
                .Select(s => new CategoryTreeModel
                {
                    children = new List<CategoryTreeModel>(),
                    folder = false,
                    key = s.Id,
                    title = s.IntentCodeEn, //s.NameCategory,
                    tooltip = s.IntentCodeEn, //s.NameCategory,
                    ParentId = s.ParentId,
                    NameCategory = s.NameCategory,
                    IntentCodeEn = s.IntentCodeEn,
                    IntentCodeVn = s.IntentCodeVn,
                    Description = s.Description

                });
            var roots = organizationalUnitModels
                .Where(w => !w.ParentId.HasValue())
                .OrderBy(o => o.IntentCodeEn); //o.NameCategory);

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
                    .OrderBy(o => o.IntentCodeEn);
                //.OrderBy(o => o.NameCategory);

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
        private List<CategotyDTO> GetWareHouseTreeModel(IEnumerable<CategotyDTO> models)
        {
            var parents = models.Where(w => string.IsNullOrEmpty(w.ParentId))
                .OrderBy(o => o.IntentCodeEn);
            // .OrderBy(o => o.NameCategory);

            var result = new List<CategotyDTO>();
            var level = 0;
            foreach (var parent in parents)
            {
                result.Add(new CategotyDTO
                {
                    Id = parent.Id,
                    ParentId = parent.ParentId,
                    NameCategory = "[" + parent.IntentCodeEn + "] " + parent.NameCategory,
                    IntentCodeEn = parent.IntentCodeEn,
                    IntentCodeVn = parent.IntentCodeVn,
                    Description = parent.Description
                });
                GetChildWareHouseTreeModel(ref models, parent.Id, ref result, level);
            }

            return result;
        }

        private void GetChildWareHouseTreeModel(ref IEnumerable<CategotyDTO> models, string parentId,
            ref List<CategotyDTO> result, int level)
        {
            level++;
            var childs = models
                .Where(w => w.ParentId == parentId)
                .OrderBy(o => o.IntentCodeEn);
            //.OrderBy(o => o.NameCategory);

            if (childs.Any())
            {
                foreach (var child in childs)
                {
                    child.NameCategory = "[" + child.IntentCodeEn + "] " + child.NameCategory;
                    result.Add(new CategotyDTO()
                    {
                        Id = child.Id,
                        ParentId = child.ParentId,
                        IntentCodeEn = GetTreeLevelString(level) + child.IntentCodeEn,
                        /*NameCategory = GetTreeLevelString(level) + child.NameCategory,*/
                        IntentCodeVn = child.IntentCodeVn,
                        Description = child.Description
                    });
                    GetChildWareHouseTreeModel(ref models, child.Id, ref result, level);
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

        public async Task<IList<Category>> GetSelect()
        {
            var res = await _category.WhereAsync(x => x.OnDelete == false && x.Inactive == true);
            return await res.Select(x => new Category()
            {
                Id = x.Id,
                IntentCodeEn = x.IntentCodeEn
            }).ToListAsync();
        }
    }
}
