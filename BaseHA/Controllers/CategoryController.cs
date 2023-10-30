using AutoMapper;
using BaseHA.Application.ModelDto.DTO;
using BaseHA.Application.ModelDto;
using BaseHA.Application.Serivce;
using BaseHA.Domain.Entity;
using BaseHA.Models.SearchModel;
using BaseHA.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using BaseHA.Core.Extensions;
using System.Diagnostics;
using BaseHA.Core.IRepositories;
using BaseHA.Core.Base;
using BaseHA.Core.ControllerBase;
using MediatR;
using static HotChocolate.ErrorCodes;

namespace BaseHA.Controllers
{
    public class CategoryController : BaseMvcController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _generic;
        private readonly IMapper _mapper;


        public CategoryController(ILogger<HomeController> logger, ICategoryService generic, IMapper mapper)
        {
            _logger = logger;
            _generic = generic;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var model = new CategorySearchModel();
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var res = await _generic.GetByIdAsync(id);
            var entity = _mapper.Map<CategoryCommands>(res);
            entity.AvailableCaegorys = await _generic.GetSelectListItem();
            return View(entity);
        }

        public async Task<IActionResult> Details(string id)
        {
            var res = await _generic.GetByIdAsync($"{id}");
            var entity = _mapper.Map<CategoryCommands>(res);
            return View(entity);
        }

        public async Task<IActionResult> DetailApi(string id)
        {
            var res = await _generic.GetByIdAsync($"{id}");
            var entity = _mapper.Map<CategoryCommands>(res);
            return Ok(new ResultMessageResponse() { 
                data=entity
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryCommands wareHouse)
        {
            if (wareHouse.Id == null)
            {
                wareHouse.Id = Guid.NewGuid().ToString();
            }
            if (wareHouse.NameCategory == null)
            {
                wareHouse.NameCategory = "ORDER";
            }
            string intentCodeVn = wareHouse.IntentCodeVn.Trim(); //xóa khoảng trắng ở đầu và cuối
            
            if (wareHouse.IntentCodeEn == null)
            {
                wareHouse.IntentCodeEn = intentCodeVn;
            }
            if (wareHouse.Inactive == false)
            {
                wareHouse.Inactive = true;
            }
           
            if (!ModelState.IsValid)
            {
                return Ok(new ResultMessageResponse()
                {
                    message = "Bạn nhập dữ liệu không đúng định dạng ! Vui lòng thử lại !",
                    success = false
                });
            }

            if (_generic.IsIntentVnDuplicate(wareHouse.IntentCodeVn))
            {
                NotifyWarning($"Mã {wareHouse.IntentCodeVn} đã tồn tại !");
                return Ok(new ResultMessageResponse()
                {
                    success = false
                });
            }

            if (_generic.IsIntentEnDuplicate(wareHouse.IntentCodeEn))
            {
                NotifyWarning($"Mã {wareHouse.IntentCodeEn} đã tồn tại !");
                return Ok(new ResultMessageResponse()
                {
                    success = false
                });
            }
            var entity = _mapper.Map<Category>(wareHouse);
            entity.IntentCodeVn= intentCodeVn;
            var res = await _generic.InsertAsync(entity);
            if (res)
                NotifySuccess("Thêm kịch bản mới thành công !");
            else
                NotifyWarning("Bạn nhập dữ liệu không đúng định dạng ! Vui lòng thử lại !");
            return Ok(new ResultMessageResponse()
            {
                success = res
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCommands wareHouse)
        {
            if (wareHouse.Id == null)
            {
                wareHouse.Id = Guid.NewGuid().ToString();
            }
            if (wareHouse.NameCategory == null)
            {
                wareHouse.NameCategory = "ORDER";
            }
            string intentCodeVn = wareHouse.IntentCodeVn.Trim(); //xóa khoảng trắng ở đầu và cuối

            if (wareHouse.IntentCodeEn == null)
            {
                wareHouse.IntentCodeEn = intentCodeVn;
            }
            if (wareHouse.Inactive == false)
            {
                wareHouse.Inactive = true;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultMessageResponse() { 
                    message = "Bạn nhập dữ liệu không đúng định dạng ! Vui lòng thử lại !"
                });
            }

            if (_generic.IsIntentVnDuplicate(wareHouse.IntentCodeVn))
            {
                return BadRequest(new ResultMessageResponse()
                {
                    message = $"Mã {wareHouse.IntentCodeVn} đã tồn tại !"
                });
            }

            if (_generic.IsIntentEnDuplicate(wareHouse.IntentCodeEn))
            {
                return BadRequest(new ResultMessageResponse()
                {
                    message = $"Mã {wareHouse.IntentCodeEn} đã tồn tại !"
                });
            }
            var entity = _mapper.Map<Category>(wareHouse);
            entity.IntentCodeVn = intentCodeVn;
            var res = await _generic.InsertAsync(entity);

            if (res)
                return Ok(true);
            else
                return BadRequest(new ResultMessageResponse()
                {
                    message = "Không thành công"
                }); 
        }

        [HttpPost]
        public async Task<IActionResult> Delete(IEnumerable<string> ids)
        {
            if (ids == null)
                return Ok(new ResultMessageResponse()
                {
                    message = "Xóa loại kịch bản thất bại !",
                    success = false
                });

            var res = await _generic.DeletesAsync(ids);
            return Ok(new ResultMessageResponse()
            {
                message = res ? "Xóa kịch bản thành công !" : "Kịch bản này đã được xóa !",
                success = res
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApi( string id)
        {

            if (id == null)
                return BadRequest(new ResultMessageResponse()
                {
                    message = "Bạn chưa chọn kịch bản"
                });
            var res = await _generic.DeleteId(id);
            if (res)
                return Ok(true);
            else
                return BadRequest(new ResultMessageResponse()
                {
                    message = " Kịch bản đã được xóa trước đó !"
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


        public async Task<IActionResult> Add()
        {
            var model = new CategoryCommands();
            model.AvailableCaegorys = await _generic.GetSelectListItem();
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(CategoryCommands unit)
        {

            var model = await _generic.GetByIdAsync(unit.Id, true);
            if (model == null)
            {
                NotifyWarning("Không tồn tại bản ghi !");
                return Ok(new ResultMessageResponse()
                {
                    message = "Không tồn tại bản ghi !",
                    success = false
                });
            }


            if (unit.IntentCodeVn != model.IntentCodeVn)
            {
                NotifyWarning(" Mã kịch bản không được thay đổi !");
                return Ok(new ResultMessageResponse()
                {
                    success = false,
                });
            }

            unit.IntentCodeEn = unit.IntentCodeVn;
            var entity = _mapper.Map(unit, model);
            var res = await _generic.UpdateAsync(entity);
            if (res)
                NotifySuccess("Sửa kịch bản thành công !");
            else
                NotifyWarning("Bạn nhập dữ liệu không đúng định dạng ! Vui lòng thử lại !");
            return Ok(new ResultMessageResponse()
            {
                success = res
            });
        }


        [HttpPost]
        public async Task<IActionResult> EditApi(CategoryCommands unit)
        {

            var model = await _generic.GetByIdAsync(unit.Id, true);
            if (model == null)
            {
                return BadRequest(new ResultMessageResponse()
                {
                    message = "Dữ liệu đã được xóa trước đó !"
                });

            }


            if (unit.IntentCodeVn != model.IntentCodeVn)
            {
                return BadRequest(new ResultMessageResponse()
                {
                    message = "Không được sửa Mã kịch bản !"
                });
            }

            unit.IntentCodeEn = unit.IntentCodeVn;
            var entity = _mapper.Map(unit, model);
            var res = await _generic.UpdateAsync(entity);
            
            if (res)
                return Ok(true);
            else
                return BadRequest(new ResultMessageResponse()
                {
                    message = "Không thành công"
                });
        }




        /// <summary>
        /// Gọi Api lấy cấu trúc cây kho
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetTree()
        {
            var res = await _generic.GetTree(2);
            IList<CategoryTreeModel> cg = new List<CategoryTreeModel>();
            foreach (var item in res)
            {
                cg.Add(item);
            }
            var all = new CategoryTreeModel()
            {
                Name = "Tất cả",
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



        #region List
        /// <summary>
        /// Lấy về danh sách dữ liệu phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Get([DataSourceRequest] DataSourceRequest request, CategorySearchModel searchModel)
        {

            searchModel.BindRequest(request);
            var data = await _generic.GetAsync(searchModel);

            var dataList = new List<CategoryCommands>();
            foreach (var item in data.Lists)
            {
                var model = _mapper.Map<CategoryCommands>(item);
                dataList.Add(model);
            }
            var result = new DataSourceResult
            {
                Data = dataList,
                Total = data.Count
            };


            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> List([DataSourceRequest] DataSourceRequest request,[FromRoute] CategorySearchModel searchModel)
        {

            searchModel.BindRequest(request);
            var data = await _generic.GetAsync(searchModel);

            var dataList = new List<CategoryCommands>();
            foreach (var item in data.Lists)
            {
                var model = _mapper.Map<CategoryCommands>(item);
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
    }
}
