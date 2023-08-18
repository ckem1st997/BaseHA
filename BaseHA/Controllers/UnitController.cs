using AutoMapper;
using BaseHA.Application.ModelDto;
using BaseHA.Application.Serivce;
using BaseHA.Domain.Entity;
using BaseHA.Models.SearchModel;
using BaseHA.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Share.BaseCore.Extensions;
using System.Diagnostics;

namespace BaseHA.Controllers
{
    public class UnitController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitService _generic;
        private readonly IMapper _mapper;

        public UnitController(ILogger<HomeController> logger, IUnitService generic, IMapper mapper)
        {
            _logger = logger;
            _generic = generic;
            _mapper = mapper;
        }

       /* private static string GenerateRandomName(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string randomName = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomName;
        }*/
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var res = await _generic.GetByIdAsync(id);
            var entity = _mapper.Map<UnitCommands>(res);
            //entity.AvailableWareHouses = await _generic.GetSelectListItem();
            return View(entity);
        }

        public async Task<IActionResult> Details(string id)
        {
            var res = await _generic.GetByIdAsync($"{id}");
            var entity = _mapper.Map<UnitCommands>(res);
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Add(UnitCommands wareHouse)
        {
            var entity = _mapper.Map<Unit>(wareHouse);
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


        public async Task<IActionResult> Add()
        {
            var model = new UnitCommands();
           // model.AvailableWareHouses = await _generic.GetSelectListItem();
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(UnitCommands unit)
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
            var data = await _generic.GetAsync(searchModel);

            var dataList = new List<UnitCommands>();
            foreach (var item in data.Lists)
            {
                var model = _mapper.Map<UnitCommands>(item);
                dataList.Add(model);
            }

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
