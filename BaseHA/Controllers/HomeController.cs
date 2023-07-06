using BaseHA.Domain.Entity;
using BaseHA.Infrastructure;
using BaseHA.Models;
using Microsoft.AspNetCore.Mvc;
using Share.BaseCore;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;
using System.Diagnostics;

namespace BaseHA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<FakeDbContext> _generic;
        private readonly IGenericRepository<Fake2DbContext> _generic2;

        public HomeController(ILogger<HomeController> logger, IGenericRepository<FakeDbContext> generic, IGenericRepository<Fake2DbContext> generic2)
        {
            _logger = logger;
            _generic = generic;
            _generic2 = generic2;
         //   this.dapper1 = EngineContext.Current.Resolve<IDapper>(DataConnectionHelper.ConnectionStringNames.Warehouse);

        }

        public async Task<IActionResult> Index()
        {
            var list = new List<Unit>();
            var l = from i in _generic.GetQueryable<Unit>() select i;
            if (l.Count()<1)
            {
                for (int i = 0; i < 10; i++)
                {
                    list.Add(new Unit()
                    {
                        Id = i.ToString(),
                        UnitName = i.ToString(),
                        Inactive = true,
                    });

                }
                await _generic.AddAsync<Unit>(list);
                await _generic.SaveChangesAsync();
            }


            return View(l.ToList());
        }

        public async Task<IActionResult> Privacy()
        {
            var list = new List<Unitssss>();
            var l = from i in _generic2.DbContext.Unitsssses select i;
            if (l.Count() < 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    list.Add(new Unitssss()
                    {
                        Id = i.ToString(),
                        UnitName = i.ToString(),
                        Inactive = true,
                    });

                }
                await _generic2.AddAsync<Unitssss>(list);
                await _generic2.SaveChangesAsync();
            }


            return View(l.ToList());
        }
         
        
        public async Task<IActionResult> Dapper()
        {
            var list = new List<Unitssss>();
            var l = from i in _generic2.DbContext.Unitsssses select i;
            if (l.Count() < 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    list.Add(new Unitssss()
                    {
                        Id = i.ToString(),
                        UnitName = i.ToString(),
                        Inactive = true,
                    });

                }
                await _generic2.AddAsync<Unitssss>(list);
                await _generic2.SaveChangesAsync();
            }


            return View(l.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}