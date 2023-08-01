using BaseHA.Domain.Entity;
using BaseHA.Infrastructure;
using BaseHA.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Share.BaseCore;
using Share.BaseCore.Attribute;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;
using Share.BaseCore.Logging;
using StackExchange.Redis;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;

namespace BaseHA.Controllers
{
    [MvcNotify(Order = 1000)] // Run last (OnResultExecuting)

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<FakeDbContext> _generic;
        public IMvcNotifier Services;
        public HomeController(ILogger<HomeController> logger, IGenericRepository<FakeDbContext> generic, IMvcNotifier services)
        {
            _logger = logger;
            _generic = generic;
            Services = services;
            //   this.dapper = EngineContext.Current.Resolve<IDapper>(DataConnectionHelper.ConnectionStringNames.Warehouse);

        }

        private static string GenerateRandomName(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string randomName = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomName;
        }
        public async Task<IActionResult> Index()
        {
            var list = new List<Unit>();
            var l = from i in _generic.GetQueryable<Unit>() select i;
            if (l.Count() < 1)
            {
                for (int i = 0; i < 100; i++)
                {
                    list.Add(new Unit()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UnitName = GenerateRandomName(10),
                        Code = GenerateRandomName(10),
                        Inactive = i % 2 == 0,
                    });

                }
                await _generic.AddAsync<Unit>(list);
            }
            await _generic.SaveChangesAsync();

            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var res = await _generic.GetByIdAsync<Unit>(id);


            return View(res);
        }



        [HttpPost]
        public async Task<IActionResult> Add(Unit unit)
        {
            await _generic.AddAsync<Unit>(unit);
            var res = await _generic.SaveChangesAsync();
            return Ok(new ResultMessageResponse()
            {
                message = "Thành công !",
                success = true
            });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await _generic.GetByIdAsync<Unit>(id);
            if (model == null)
                return Ok(false);
            _generic.Remove<Unit>(model);
            var res = await _generic.SaveChangesAsync();
            return Ok(res);
        }


        public async Task<IActionResult> Add()
        {
            return View(new Unit());
        }



        [HttpPost]
        public async Task<IActionResult> Edit(string id, Unit unit)
        {
            var model = await _generic.GetByIdAsync<Unit>(id);
            if (model == null)
                return Ok(false);
            var res = await _generic.UpdateAsync<Unit>(unit);
            return Ok(res);
        }


        public async Task<IActionResult> Privacy()
        {
            return View();
        }



        #region List
        /// <summary>
        /// Lấy về danh sách dữ liệu phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Get([DataSourceRequest] DataSourceRequest request, UnitSearchModel searchModel)
        {

            searchModel.BindRequest(request);

            var res = new List<Unit>();

            var l = from i in _generic.GetQueryable<Unit>() select i;
            if (!string.IsNullOrEmpty(searchModel.Keywords))
                l = from aa in l where aa.UnitName.Contains(searchModel.Keywords) || aa.Code.Contains(searchModel.Keywords) select aa;

            var data = await l.Skip((searchModel.PageIndex - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToListAsync();

            var result = new DataSourceResult
            {
                Data = data,
                Total = await l.CountAsync()
            };

            return Ok(result);
        }
        #endregion



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}