using AutoMapper;
using BaseHA.Application.ModelDto;
using BaseHA.Application.Serivce;
using BaseHA.Domain.Entity;
using BaseHA.Models;
using BaseHA.Models.SearchModel;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using BaseHA.Core.Extensions;
using BaseHA.Core.ControllerBase;

namespace BaseHA.Controllers
{

    public class AnswerController : BaseMvcController
    {
        private readonly ILogger<AnswerController> _logger;
        private readonly IAnswerSerive _generic;
        private readonly IMapper _mapper;

        public AnswerController(ILogger<AnswerController> logger, IAnswerSerive generic, IMapper mapper)
        {
            _logger = logger;
            _generic = generic;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var model = new AnswerSearchModel();
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var res = await _generic.GetByIdAsync(id);
            var entity = _mapper.Map<AnswerCommands>(res);
            return View(entity);
        }
        public async Task<IActionResult> Details(string id)
        {
            var res = await _generic.GetByIdAsync($"{id}");
            var entity = _mapper.Map<AnswerCommands>(res);
            return View(entity);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AnswerCommands wareHouse)
        {
            if (wareHouse.CategoryId == null)
            {
                return BadRequest(new ResultMessageResponse()
                {
                    message = $"Bạn chưa lựa chọn Tên kịch bản !",
                    success = false
                });
            }
            if (wareHouse.Id == null)
            {
                wareHouse.Id = Guid.NewGuid().ToString();
            }
            if (wareHouse.Inactive == false)
            {
                wareHouse.Inactive = true;
            }
            var entity = _mapper.Map<Answer>(wareHouse);
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
        public async Task<IActionResult> Add(string CategoryId)
        {
            var model = new AnswerCommands();
            model.CategoryId = CategoryId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AnswerCommands unit)
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

        #region List
        /// <summary>
        /// Lấy về danh sách dữ liệu phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Get([DataSourceRequest] DataSourceRequest request, AnswerSearchModel searchModel)
        {

            searchModel.BindRequest(request);
            var data = await _generic.GetAsync(searchModel);
            var list = new List<AnswerCommands>();
            foreach (var command in data?.Lists)
            {
                var model = _mapper.Map<AnswerCommands>(command);
                if (model != null)
                    list.Add(model);
            }

            var result = new DataSourceResult
            {
                Data = list,
                Total = data.Count
            };

            return Ok(result);
        }
        #endregion


    }

}
