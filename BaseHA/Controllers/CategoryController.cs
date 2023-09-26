using AutoMapper;
using BaseHA.Application.ModelDto.DTO;
using BaseHA.Application.ModelDto;
using BaseHA.Application.Serivce;
using BaseHA.Domain.Entity;
using BaseHA.Models.SearchModel;
using BaseHA.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Share.BaseCore.Extensions;
using System.Diagnostics;
using Share.BaseCore.IRepositories;
using Share.BaseCore.Base;

namespace BaseHA.Controllers
{
    public class CategoryController : Controller
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

        [HttpPost]
        public async Task<IActionResult> Add(CategoryCommands wareHouse)
        {
            if(wareHouse.IntentCodeEn==null)
            {
                wareHouse.IntentCodeEn = wareHouse.IntentCodeVn;
            }

            if (ModelState.IsValid)
            {
                if (_generic.IsIntentVnDuplicate( wareHouse.IntentCodeVn))
                {
                    
                    ModelState.AddModelError("IntentCodeVn", "Mã bằng tiếng Việt đã tồn tại");
                    //return View(wareHouse);
                    return Ok(new ResultMessageResponse()
                    {
                        message = $"Mã {wareHouse.IntentCodeVn} bằng tiếng Việt đã tồn tại !",
                        success = false
                    });
                }
                else
                {
                    if (_generic.IsIntentEnDuplicate(wareHouse.IntentCodeEn))
                    {
                        ModelState.AddModelError("IntentCodeEn", "Mã bằng tiếng Anh đã tồn tại");
                        //return View(wareHouse);
                        return Ok(new ResultMessageResponse()
                        {
                            message = "Mã bằng tiếng Anh đã tồn tại !",
                            success = false
                        });
                    }
                }

                var entity = _mapper.Map<Category>(wareHouse);
                var res = await _generic.InsertAsync(entity);
                return Ok(new ResultMessageResponse()
                {
                    message = res ? "Thành công !" : "Thất bại !",
                    success = res
                });
            }
            /* ViewBag.ErrorMessage = "Dữ liệu không hợp lệ";
             return View(wareHouse);*/
            return Ok(new ResultMessageResponse()
            {
                message = "Thất bại !",
                success = false
            });
        }

       /* public  bool IsIntentEnDuplicate(string intent)
        {
            bool isDuplicate = _category.Table.Any(e => e.IntentCodeEn == intent);
            return isDuplicate;
        }*/



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
            var model = new CategoryCommands();
            model.AvailableCaegorys = await _generic.GetSelectListItem();
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(CategoryCommands unit)
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
        #endregion      
    }
}
