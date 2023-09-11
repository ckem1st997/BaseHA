using AutoMapper;
using BaseHA.Application.AutoMapper.CategoryTbs;
using BaseHA.Application.ModelDto;
using BaseHA.Application.Serivce;
using BaseHA.Domain.Entity;
using BaseHA.Models;
using BaseHA.Models.SearchModel;
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
        private readonly ICategoryTbService _generic;
        private readonly ICategoryTbService _context;

        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, ICategoryTbService generic, IMapper mapper)
        {
            _logger = logger;
            _generic = generic;
            _mapper = mapper;
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

        public async Task<IActionResult> Edit(string id){
            var res = await _generic.GetByIdAsync(id);
            var entity = _mapper.Map<CategoryTbCommands>(res);
            //entity.AvailableCategoryTb = await _generic.GetSelectListItem();
            return View(entity);
        }

        public async Task<IActionResult> Details(string id)
        {
            var res = await _generic.GetByIdAsync($"{id}");
            var entity = _mapper.Map<CategoryTbCommands>(res);
            return View(entity);
        }


        public async Task<IActionResult> Add()
        {
            var model = new CategoryTbCommands();
            // model.AvailableCategoryTb = await _generic.GetSelectListItem();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryTbCommands category){

            var entity = _mapper.Map<CategoryTb>(category);
            var res = await _generic.InsertAsync(entity);

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

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryTbCommands unit)
        {
            var model = await _generic.GetByIdAsync(unit.Id, true);
            if (model == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Không tồn tại bản ghi !",
                    success = false
                });
            var entity = _mapper.Map(unit, model);
            var res = await _generic.UpdateAsync(entity);
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

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Get([DataSourceRequest] DataSourceRequest request, CategoryTbSearchModel searchModel)
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



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}