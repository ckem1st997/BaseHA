using BaseHA.Domain.Entity;
using BaseHA.Models;
using BaseHA.Serivce;
using Kendo.Mvc.UI;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Share.BaseCore;
using Share.BaseCore.Attribute;
using Share.BaseCore.Base;
using Share.BaseCore.Extensions;
using Share.BaseCore.IRepositories;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;

namespace BaseHA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWareHouseService _generic;
        public HomeController(ILogger<HomeController> logger, IWareHouseService generic)
        {
            _logger = logger;
            _generic = generic;
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
            var res = await _generic.GetByIdAsync(id);
            return View(res);
        }

        public async Task<IActionResult> Details(string id)
        {
            var res = await _generic.GetByIdAsync($"{id}");
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Add(WareHouse unit)
        {
            var res = await _generic.InsertAsync(unit);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
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

            var res = await _generic.DeletesAsync(ids);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
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
            var res = await _generic.ActivatesAsync(ids, active);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
            });
        }


        public async Task<IActionResult> Add()
        {
            return View(new WareHouse());
        }



        [HttpPost]
        public async Task<IActionResult> Edit(WareHouse unit)
        {
            var model = await _generic.GetByIdAsync(unit.Id);
            if (model == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Không tồn tại bản ghi !",
                    success = false
                });
            var res = await _generic.UpdateAsync(unit);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
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
        public async Task<IActionResult> Get([DataSourceRequest] DataSourceRequest request, WareHouseSearchModel searchModel)
        {

            searchModel.BindRequest(request);
            var data = await _generic.GetAsync(searchModel);

            var result = new DataSourceResult
            {
                Data = data.Lists,
                Total = data.Count
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