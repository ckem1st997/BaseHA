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
    public class IntentController : Controller
    {
        private readonly ILogger<IntentController> _logger;
        private readonly IIntentService _intent;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public IntentController(ILogger<IntentController> logger, IIntentService intent, ICategoryService category, IMapper mapper)
        {
            _logger = logger;
            _intent = intent;
            _categoryService = category;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var res = await _intent.GetByIdAsync(id);
            //res.AvailableCategories = await _intent.GetSelectListItem();
            var entity = _mapper.Map<IntentCommands>(res);
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IntentCommands intent)
        {
            var model = await _intent.GetByIdAsync(intent.Id, true);
            if (model == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Không tồn tại bản ghi !",
                    success = false
                });
            var entity = _mapper.Map(intent, model);
            var res = await _intent.UpdateAsync(entity);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
            });
        }

        public async Task<IActionResult> Details(string id)
        {
            var res = await _intent.GetByIdAsync($"{id}");
            var entity = _mapper.Map<IntentCommands>(res);
            return View(entity);
        }
        public async Task<IActionResult> Add()
        {
            var model = new IntentCommands();
            model.AvailableCategory = await _intent.GetSelectListItem();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(IntentCommands intents)
        {
            if (intents.Id == null)
            {
                intents.Id= Guid.NewGuid().ToString();
            }
            var entity = _mapper.Map<Intent>(intents);
            var res = await _intent.InsertAsync(entity);
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

            var res = await _intent.DeletesAsync(ids);
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
            var res = await _intent.ActivatesAsync(ids, active);
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
        public async Task<IActionResult> Get([DataSourceRequest] DataSourceRequest request, IntentSearchModel searchModel)
        {

            searchModel.BindRequest(request);
            var data = await _intent.GetAsync(searchModel);

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
