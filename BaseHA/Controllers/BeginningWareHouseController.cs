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
    public class BeginningWareHouseController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBeginningWareHouseService _generic;
        private readonly IMapper _mapper;

        public BeginningWareHouseController(ILogger<HomeController> logger, IBeginningWareHouseService generic, IMapper mapper)
        {
            _logger = logger;
            _generic = generic;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var res = await _generic.GetByIdAsync(id);
            var entity = _mapper.Map<BeginningCommands>(res);
            return View(entity);
        }

        public async Task<IActionResult> Details(string id)
        {
            var res = await _generic.GetByIdAsync($"{id}");
            var entity = _mapper.Map<BeginningCommands>(res);
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BeginningCommands wareHouse)
        {
            var entity = _mapper.Map<BeginningWareHouse>(wareHouse);
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
            var model = new BeginningCommands();
            model.AvailableWareHouses = await _generic.GetSelectListWareHouse();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(BeginningWareHouse model)
        {
           /* var model = await _generic.GetByIdAsync(unit.Id, true);

            if (model == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Không tồn tại bản ghi !",
                    success = false
                });
            var entity = _mapper.Map(unit, model);*/

            var res = await _generic.UpdateAsync(model);
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
        public async Task<IActionResult> Get([DataSourceRequest] DataSourceRequest request, BeginningWareHouseModel searchModel)
        {

            searchModel.BindRequest(request);

            var data = await _generic.GetAsync(searchModel);
            var dataList = new List<BeginningCommands>();
            
            foreach (var item in data.Lists)
            {
                var model = _mapper.Map<BeginningCommands>(item);
                dataList.Add(model);
            }
            var result = new DataSourceResult
            {
                Data = dataList,
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