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
using Share.BaseCore.Notifier;

namespace BaseHA.Controllers
{
    public class IntentController : Controller
    {
        private readonly ILogger<IntentController> _logger;
        private readonly IIntentService _intent;
        private readonly ICategoryService _category;
        private readonly IAnswerService _answer;
        private readonly IMapper _mapper;
        private readonly IMvcNotifier _mvcNotifier;

        public IntentController(ILogger<IntentController> logger, IIntentService intent, ICategoryService category, IAnswerService answer, IMapper mapper, IMvcNotifier mvcNotifier)
        {
            _logger = logger;
            _intent = intent;
            _category = category;
            _answer = answer;
            _mapper = mapper;
            _mvcNotifier = mvcNotifier;
        }

        public async Task<IActionResult> Index()
        {
            // var model = new CategorySearchModel();
            //return View(model);
            return View();
        }
        public async Task<IActionResult> IndexTreeAsync()
        {
            var model = new CategorySearchModel();
            var resCategory = await _category.GetSelect();
            ViewData["category"] = resCategory.Select(x => new CategotyDTO()
            {
                Id = x.Id,
                IntentCodeEn = x.IntentCodeEn
            });
            ViewData["defaultCategory"] = resCategory.FirstOrDefault();
            return View(model);
            //return View();
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
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(IntentCommands intents)
        {
            if (intents.Id == null)
            {
                intents.Id = Guid.NewGuid().ToString();
            }
            var entity = _mapper.Map<Intent>(intents);
            var res = await _intent.InsertAsync(entity);
            _mvcNotifier.Add(MvcNotifyType.Success, res ? "Thành công !" : "Thất bại !");
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
            });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(IntentCommands intent)
        {
            if (intent.Id == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Thất bại !",
                    success = false
                });

            var res = await _intent.DeleteAsyncID(intent.Id);
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
