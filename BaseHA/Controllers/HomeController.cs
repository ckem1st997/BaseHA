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
using Share.BaseCore.BaseNop;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;
using StackExchange.Redis;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;

namespace BaseHA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositoryEF<WareHouse> _generic;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _generic = EngineContext.Current.Resolve<IRepositoryEF<WareHouse>>(DataConnectionHelper.ConnectionStringNames.Warehouse);
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
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var res = await _generic.GetFirstAsyncAsNoTracking(id);
            return View(res);
        }

        public async Task<IActionResult> Details(string id)
        {
            var res = await _generic.GetFirstAsyncAsNoTracking(id);
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Add(WareHouse unit)
        {
            await _generic.AddAsync(unit);
            var res = await _generic.SaveChangesConfigureAwaitAsync();
            return Ok(new ResultMessageResponse()
            {
                message = "Thành công !",
                success = res > 0
            });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(IEnumerable<string> ids)
        {
            if (ids == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Thất bại !",
                    success = false
                });

            var model = _generic.GetList(x => ids.Contains(x.Id));


            _generic.Delete(model);
            var res = await _generic.SaveChangesConfigureAwaitAsync();
            return Ok(new ResultMessageResponse()
            {
                message = "Thành công !",
                success = res > 0
            });
        }   
        
        
        [HttpPost]
        public async Task<IActionResult> Activates(IEnumerable<string> ids, bool active)
        {
            if (ids == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Thất bại !",
                    success = false
                });

            var model = _generic.GetList(x => ids.Contains(x.Id));
            var listUpdate = new List<WareHouse>();
            foreach (var item in model.ToList())
            {
                item.Inactive=active;
                listUpdate.Add(item);
            }
            _generic.Update(listUpdate);
            var res = await _generic.SaveChangesConfigureAwaitAsync();
            return Ok(new ResultMessageResponse()
            {
                message = "Thành công !",
                success = res > 0
            });
        }


        public async Task<IActionResult> Add()
        {
            return View(new WareHouse());
        }



        [HttpPost]
        public async Task<IActionResult> Edit(WareHouse unit)
        {
            var model = await _generic.GetFirstAsync(unit.Id);
            if (model == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Không tồn tại bản ghi !",
                    success = false
                });
            _generic.Update(unit);
            var res = await _generic.SaveChangesConfigureAwaitAsync();
            return Ok(new ResultMessageResponse()
            {
                message = "Thành công !",
                success = res > 0
            });
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

            var l = from i in _generic.Table select i;
            if (!string.IsNullOrEmpty(searchModel.Keywords))
                l = from aa in l where aa.Name.Contains(searchModel.Keywords) || aa.Code.Contains(searchModel.Keywords) select aa;

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