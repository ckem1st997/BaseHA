using AutoMapper;
using BaseHA.Application.ModelDto;
using BaseHA.Application.Serivce;
using BaseHA.Domain.Entity;
using BaseHA.Models;
using BaseHA.Models.SearchModel;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BaseHA.Core.Base;
using BaseHA.Core.Extensions;
using BaseHA.Core.IRepositories;
using BaseHA.Core.ControllerBase;

namespace BaseHA.Controllers
{
    public class IntentController : BaseMvcController
    {
        private readonly ILogger<IntentController> _logger;
        private readonly IIntentService _generic;
        private readonly IMapper _mapper;
        

        public IntentController(ILogger<IntentController> logger, IIntentService generic, IMapper mapper)
        {
            _logger = logger;
            _generic = generic;
            _mapper = mapper;
            
        }

        public IActionResult Index()
        {
            var model = new IntentSearchModel();
            return View(model);
        }      


        public async Task<IActionResult> Edit(string id)
        {
            var res = await _generic.GetByIdAsync(id);
            var entity = _mapper.Map<IntentCommands>(res);
            return View(entity);
        }

        public async Task<IActionResult> Details(string id)
        {
            var res = await _generic.GetByIdAsync($"{id}");
            var entity = _mapper.Map<IntentCommands>(res);
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Add(IntentCommands wareHouse)
        {
            if(wareHouse.CategoryId==null)
            {
                return BadRequest(new ResultMessageResponse()
                {
                    message = $"Bạn chưa lựa chọn Loại kịch bản !",
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
            var entity = _mapper.Map<Intent>(wareHouse);
            var res = await _generic.InsertAsync(entity);
            if (res)
                NotifySuccess("Thêm ý định mới thành công !");
            else
                NotifyWarning("Bạn nhập dữ liệu không đúng định dạng ! Vui lòng thử lại !");
            return Ok(new ResultMessageResponse()
            {
                success = res
            });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(IEnumerable<string> ids)
        {
            if (ids == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Xóa ý định thất bại !",
                    success = false
                });

            var res = await _generic.DeletesAsync(ids);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Xóa ý định thành công !" : "Kịch bản này đã được xóa !",
                success = res
            });
        }


        [HttpPost]
        public async Task<IActionResult> Activates(IEnumerable<string> ids, bool active)
        {
            if (ids == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Thay đổi trạng thái thất bại !",
                    success = false
                });
            var res = await _generic.ActivatesAsync(ids, active);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Thay đổi trạng thái thành công !" : "Sản phẩm đã được kích hoạt! Vui lòng thử lại !",
                success = res
            });
        }
        


        public async Task<IActionResult> Add(string CategoryId)
        {
            var model = new IntentCommands();
            model.CategoryId = CategoryId;
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(IntentCommands unit)
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
            if (res)
                NotifySuccess("Sửa ý định thành công !");
            else
                NotifyWarning("Bạn nhập dữ liệu không đúng định dạng ! Vui lòng thử lại !");
            return Ok(new ResultMessageResponse()
            {
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
        public async Task<IActionResult> Get([DataSourceRequest] DataSourceRequest request, IntentSearchModel searchModel)
        {

            searchModel.BindRequest(request);
            var data = await _generic.GetAsync(searchModel);
            var list = new List<IntentCommands>();
            foreach (var command in data?.Lists)
            {
                var model = _mapper.Map<IntentCommands>(command);
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

