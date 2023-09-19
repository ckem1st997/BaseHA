using BaseHA.Application.ModelDto;
using BaseHA.Domain.Entity;
using BaseHA.Models.SearchModel;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nest;
using Share.BaseCore.Base;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BaseHA.Application.Serivce
{
    public interface IAnswerService
    {
            Task<bool> InsertAsync(Answer entities);

            Task<bool> InsertWHAsync(IEnumerable<Answer> entities);

            Task<bool> UpdateAsync(Answer entity);

            Task<bool> DeletesAsync(IEnumerable<string> ids);
            Task<bool> DeleteAsyncID(string id);

            Task<PagedList<Answer>> GetAsync(AnswerSearchModel ctx);

            Task<Answer> GetByIdAsync(string id, bool tracking = false);

            Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active);

            Task<IList<SelectListItem>> GetSelectListItem();
     }

    public class AnswerService : IAnswerService
    {
            private readonly IRepositoryEF<Answer> _answer;
        private readonly IRepositoryEF<Intent> _intent;

        public AnswerService()
            {
                _answer = EngineContext.Current.Resolve<IRepositoryEF<Answer>>(DataConnectionHelper.ConnectionStringNames.Warehouse);
            _intent = EngineContext.Current.Resolve<IRepositoryEF<Intent>>(DataConnectionHelper.ConnectionStringNames.Warehouse);
        }

            public async Task<bool> ActivatesAsync(IEnumerable<string> ids, bool active)
            {
                if (ids == null)
                    throw new ArgumentNullException(nameof(ids));
                var list = await _answer.WhereTracking(x => ids.Contains(x.Id) && !x.OnDelete).ToListAsync();
                if (list == null)
                {
                    return false;
                }
                list.ForEach(x => x.Inactive = active);
                return await _answer.SaveChangesConfigureAwaitAsync() > 0;
            }

        public async Task<bool> DeleteAsyncID(string id)
        {
            if (id == null)
                throw new NotImplementedException();

            await _answer.DeteleSoftDelete(id);
            return await _answer.SaveChangesConfigureAwaitAsync() > 0;
        }

        public async Task<bool> DeletesAsync(IEnumerable<string> ids)
            {
                if (ids == null)
                    throw new NotImplementedException();

                await _answer.DeteleSoftDelete(ids);
                return await _answer.SaveChangesConfigureAwaitAsync() > 0;
            }

        public async Task<PagedList<Answer>> GetAsync(AnswerSearchModel ctx)
        {
            var list = from cate in _answer.Table where cate.OnDelete == false select cate; //.Include("Intent")
            if (!string.IsNullOrEmpty(ctx.Keywords))
            {
                list = from c in list
                       where c.IntentCodeEn.Contains(ctx.Keywords) || c.AnswerVn.Contains(ctx.Keywords)
                       select c;
            }


           /* var query = from q in list
                        join i in _answer.Table on q.IntentId equals i.Intent.Id
                        select new Answer
                        {
                            Id = q.Id,
                            IntentId = q.IntentId, // i.Intent.IntentCodeEn ,
                            //IntentCodeEn = i.Intent.IntentCodeEn,
                            AnswerVn = q.AnswerVn
                        };*/


            PagedList<Answer> res = new PagedList<Answer>();
            await res.Result(ctx.PageSize, (ctx.PageIndex - 1) * ctx.PageSize, list);

            return res;

           /* var query = from a in _answer.Table
                        join i in _intent.Table
                    on a.IntentId equals i.Id into intentjoin
                        select new
                        {
                            id = a.Id,
                            intnentId = a.IntentId,
                            answerVn = a.AnswerVn,
                            intnetCodeEN = intentjoin.FirstOrDefault().IntentCodeEn
                        };
            var result = query.AsList().Select(e => new Answer
            {
                Id = e.id,
                IntentId = e.intnentId,
                AnswerVn = e.answerVn,
                IntentCodeEn = e.intnetCodeEN

            });
            PagedList<Answer> res1 = new PagedList<Answer>();
            await res1.Result(ctx.PageSize, (ctx.PageIndex - 1) * ctx.PageSize, result);

            return res1;*/
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

        public async Task<Answer> GetByIdAsync(string id, bool tracking = false)
            {
                if (id == null)
                    throw new NotImplementedException();
                return await _answer.GetByIdsync(id, Tracking: tracking);
            }

        public Task<IList<SelectListItem>> GetSelectListItem()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertAsync(Answer entities)
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));
                await _answer.AddAsync(entities);
                return await _answer.SaveChangesConfigureAwaitAsync() > 0;
            }

            public async Task<bool> InsertWHAsync(IEnumerable<Answer> entities)
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));
                _answer.Update(entities);
                return await _answer.SaveChangesConfigureAwaitAsync() > 0;
            }

            public async Task<bool> UpdateAsync(Answer entity)
            {
            /*var list = from cate in _answer.Table select cate;
            var query= from q in list where q.Id == entity.Id 
                    
                       select new Answer
                       {
                           Id = q.Id,
                           IntentId = entity.IntentId,
                           //IntentCodeEn = i.Intent.IntentCodeEn,
                           AnswerVn = q.AnswerVn
                       };*/
            if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

            _answer.Update(entity);
                return await _answer.SaveChangesConfigureAwaitAsync() > 0;
            }
        }
    }

