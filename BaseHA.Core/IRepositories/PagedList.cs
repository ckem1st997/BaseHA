using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseHA.Core.IRepositories
{
    public class PagedList<T> where T : class
    {
        public PagedList()
        {
        }

        public IList<T>? Lists { get; set; }
        public int Count { get; set; }

        public async Task Result(int take, int skip, IQueryable<T> values)
        {
            Lists = await values.Skip(skip).Take(take).ToListAsync();
            Count = values.Count();
        }



    }
}
