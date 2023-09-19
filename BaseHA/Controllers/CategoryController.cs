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
using BaseHA.Application.ModelDto.DTO;

namespace BaseHA.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _category;
        private readonly IMapper _mapper;
        private readonly IIntentService _intent;
        private readonly IAnswerService _answer;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService category, IIntentService intent, IAnswerService answer, IMapper mapper)
        {
            _logger = logger;
            _category = category;
            _mapper = mapper;
            _intent = intent;
            _answer = answer;
        }
        public IActionResult Index()
        {
            var model = new CategorySearchModel();
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var res = await _category.GetByIdAsync(id);
            var entity = _mapper.Map<CategoryCommands>(res);
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryCommands unit)
        {
            var model = await _category.GetByIdAsync(unit.Id, true);
            if (model == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Không tồn tại bản ghi !",
                    success = false
                });
            var entity = _mapper.Map(unit, model);
            var res = await _category.UpdateAsync(entity);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
            });
        }

        public async Task<IActionResult> Details(string id)
        {
            var res = await _category.GetByIdAsync($"{id}");
            var entity = _mapper.Map<CategoryCommands>(res);
            return View(entity);
        }

        public async Task<IActionResult> Add()
        {
            var model = new CategoryCommands();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(CategoryCommands category)
        {
            if (category.Id == null) {
                category.Id = Guid.NewGuid().ToString();
            }
            var entity = _mapper.Map<Category>(category);
            var res = await _category.InsertAsync(entity);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
            });
        }


        /*[HttpPost]
        public async Task<IActionResult> Delete000(IEnumerable<string> ids)
        {
            if (ids == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Thất bại !",
                    success = false
                });

            var res = await _category.DeletesAsync(ids);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
            });
        }*/
        [HttpPost]
        public async Task<IActionResult> Delete(CategoryCommands category)
        {
            if (category.Id == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Thất bại !",
                    success = false
                });

            var res = await _category.DeleteAsyncID(category.Id);
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
            var res = await _category.ActivatesAsync(ids, active);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
            });
        }



        /// <summary>
        /// Gọi Api lấy cấu trúc cây kho
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetTree()
        {
            var res = await _category.GetTree(2);
            IList<CategoryTreeModel> cg = new List<CategoryTreeModel>();
            foreach (var item in res)
            {
                cg.Add(item);
            }
            var all = new CategoryTreeModel()
            {
                NameCategory = "Tất cả",
                key = "",
                title = "Tất cả",
                tooltip = "Tất cả",
                children = cg,
                level = 1,
                expanded = true
            };
            res.Clear();
            res.Add(all);

            return Ok(res);
        }


        #region List Category
        /// <summary>
        /// Lấy về danh sách dữ liệu phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> GetCategory([DataSourceRequest] DataSourceRequest request, CategorySearchModel searchModel)
        {

            searchModel.BindRequest(request);
            var data = await _category.GetAsync(searchModel);

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

        #region List Intent
        /// <summary>
        /// Lấy về danh sách dữ liệu phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> GetIntent([DataSourceRequest] DataSourceRequest request, IntentSearchModel searchModel)
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

        #region List Answer
        /// <summary>
        /// Lấy về danh sách dữ liệu phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> GetAnswer([DataSourceRequest] DataSourceRequest request, AnswerSearchModel searchModel)
        {

            searchModel.BindRequest(request);
            var data = await _answer.GetAsync(searchModel);

            var result = new DataSourceResult
            {
                Data = data.Lists,
                Total = data.Count
            };
            //var entity = _mapper.Map<AnswerCommands>(result);

            return Ok(result);
        }
        #endregion


    }
}
