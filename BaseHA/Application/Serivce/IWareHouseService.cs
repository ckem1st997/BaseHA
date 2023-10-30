using BaseHA.Application.ModelDto.DTO;
using BaseHA.Domain.Entity;
using BaseHA.Models.SearchModel;
using Dapper;
using GreenDonut;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.EntityFrameworkCore;
using Nest;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using BaseHA.Core.Extensions;
using BaseHA.Core.IRepositories;
using BaseHA.Core.Base;

namespace BaseHA.Application.Serivce
{
    public interface IWareHouseService
    {
        Task<bool> InsertAsync(WareHouse entity);

        Task<bool> InsertWHAsync(IEnumerable<WareHouse> entities);

        Task<bool> UpdateAsync(WareHouse entity);

        Task<bool> DeletesAsync(IEnumerable<string> ids);

        Task<PagedList<WareHouse>> GetAsync(WareHouseSearchModel ctx);

        Task<WareHouse> GetByIdAsync(string id, bool tracking = false);

        Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active);
        Task<IList<SelectListItem>> GetSelectListItem();
        Task<IList<WareHouseTreeModel>> GetTree(int? expandLevel);
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

            //var list = await _generic.GetQueryable().Where(x => ids.Contains(x.Id) && x.OnDelete == false).ToListAsync();

            //if (list == null)
            //    throw new ArgumentNullException("list is null !");

            //list.ForEach(x => x.OnDelete = true);
            //  _generic.Update(list);
            await _generic.DeteleSoftDelete(ids);
            return await _generic.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<PagedList<WareHouse>> GetAsync(WareHouseSearchModel ctx)
        {
            var l = from i in _generic.Table select i;

            if (!string.IsNullOrEmpty(ctx.Keywords))
                l = from aa in l where aa.Name.Contains(ctx.Keywords) || aa.Code.Contains(ctx.Keywords) select aa;

            if (!string.IsNullOrEmpty(ctx.WareHouseId))
            {
                // lấy các con của id và tìm kiếm

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("WITH cte (Id, Name, ParentId) AS (");
                queryBuilder.Append("    SELECT");
                queryBuilder.Append("        wh.Id,");
                queryBuilder.Append("        wh.Name,");
                queryBuilder.Append("        wh.ParentId");
                queryBuilder.Append("    FROM");
                queryBuilder.Append("        WareHouse wh");
                queryBuilder.Append("    WHERE");
                queryBuilder.Append("        wh.ParentId = '" + ctx.WareHouseId + "'");
                queryBuilder.Append("    UNION ALL");
                queryBuilder.Append("    SELECT");
                queryBuilder.Append("        p.Id,");
                queryBuilder.Append("        p.Name,");
                queryBuilder.Append("        p.ParentId");
                queryBuilder.Append("    FROM");
                queryBuilder.Append("        WareHouse p");
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
                departmentIds.Add(ctx.WareHouseId);
                if (departmentIds != null && departmentIds.Any())
                    l = from aa in l where departmentIds.Contains(aa.Id) select aa;
            }
            PagedList<WareHouse> res = new PagedList<WareHouse>();
            await res.Result(ctx.PageSize, (ctx.PageIndex - 1) * ctx.PageSize, l);
            return res;
        }

        public async Task<WareHouse> GetByIdAsync(string id, bool tracking = false)
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

        public async Task<IList<WareHouseTreeModel>> GetTree(int? expandLevel)
        {
            expandLevel = expandLevel ?? 1;
            var qq = new Queue<WareHouseTreeModel>();
            var lstCheck = new List<WareHouseTreeModel>();
            var result = new List<WareHouseTreeModel>();
            string sql = "select Id,ParentId,Code,Name from WareHouse where Inactive =@active and OnDelete=0 ";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@active", 1);
            var getAll = await _generic.QueryAsync<WareHouseDTO>(sql, parameter, CommandType.Text);
            var organizationalUnitModels = getAll
                .Select(s => new WareHouseTreeModel
                {
                    children = new List<WareHouseTreeModel>(),
                    folder = false,
                    key = s.Id,
                    title = s.Name,
                    tooltip = s.Name,
                    Path = s.Path,
                    ParentId = s.ParentId,
                    Code = s.Code,
                    Name = s.Name
                });
            var roots = organizationalUnitModels
                .Where(w => !w.ParentId.HasValue())
                .OrderBy(o => o.Name);

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

        private List<WareHouseDTO> GetWareHouseTreeModel(IEnumerable<WareHouseDTO> models)
        {
            var parents = models.Where(w => string.IsNullOrEmpty(w.ParentId))
                .OrderBy(o => o.Name);

            var result = new List<WareHouseDTO>();
            var level = 0;
            foreach (var parent in parents)
            {
                result.Add(new WareHouseDTO
                {
                    Id = parent.Id,
                    ParentId = parent.ParentId,
                    Name = "[" + parent.Code + "] " + parent.Name,
                    Code = parent.Code
                });
                GetChildWareHouseTreeModel(ref models, parent.Id, ref result, level);
            }

            return result;
        }

        private void GetChildWareHouseTreeModel(ref IEnumerable<WareHouseDTO> models, string parentId,
            ref List<WareHouseDTO> result, int level)
        {
            level++;
            var childs = models
                .Where(w => w.ParentId == parentId)
                .OrderBy(o => o.Name);

            if (childs.Any())
            {
                foreach (var child in childs)
                {
                    child.Name = "[" + child.Code + "] " + child.Name;
                    result.Add(new WareHouseDTO()
                    {
                        Id = child.Id,
                        ParentId = child.ParentId,
                        Name = GetTreeLevelString(level) + child.Name,
                        Code = child.Code
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
    }
}
