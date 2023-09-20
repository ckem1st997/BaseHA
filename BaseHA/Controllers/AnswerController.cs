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
    public class AnswerController : Controller
    {
        private readonly ILogger<AnswerController> _logger;
        private readonly IAnswerService _answer;
        private readonly IMapper _mapper;

        public AnswerController(ILogger<AnswerController> logger, IAnswerService answer, IMapper mapper)
        {
            _logger = logger;
            _answer = answer;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var res = await _answer.GetByIdAsync(id);
            var entity = _mapper.Map<AnswerCommands>(res);
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AnswerCommands unit)
        {
            var model = await _answer.GetByIdAsync(unit.Id, true);
            if (model == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Không tồn tại bản ghi !",
                    success = false
                });
            var entity = _mapper.Map(unit, model);
            var res = await _answer.UpdateAsync(entity);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
            });
        }

        public async Task<IActionResult> Details(string id)
        {
            var res = await _answer.GetByIdAsync($"{id}");
            var entity = _mapper.Map<AnswerCommands>(res);
            return View(entity);
        }

        public async Task<IActionResult> Add()
        {
            var model = new AnswerCommands();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AnswerCommands answer)
        {
            if (answer.Id == null)
            {
                answer.Id = Guid.NewGuid().ToString();
            }

            var entity = _mapper.Map<Answer>(answer);
            var res = await _answer.InsertAsync(entity);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
            });
        }


        [HttpPost]
        public async Task<IActionResult> Delete000(IEnumerable<string> ids)
        {
            if (ids == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Thất bại !",
                    success = false
                });

            var res = await _answer.DeletesAsync(ids);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thành công !" : "Thất bại !",
                success = res
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Answer answer)
        {
            if (answer.Id == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Thất bại !",
                    success = false
                });

            var res = await _answer.DeleteAsyncID(answer.Id);
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
            var res = await _answer.ActivatesAsync(ids, active);
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
