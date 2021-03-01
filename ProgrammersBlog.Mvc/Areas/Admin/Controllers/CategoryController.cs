using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Entities.DTOs;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Extensions;
using ProgrammersBlog.Shared.Utilities.Resuslts.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAll();


            //category manager içinde error alırsam da succes alırsam da data gönderdiğimde hata vermiycek şekilde ayarladığım için direkt data gönderebilirim.
            return View(result.Data);

        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.Add(categoryAddDto, "Esra Ünal");
                if (result.ResultStatus==ResultStatus.Success)
                {
                    //json'a dönüştükten sonra ben almak istiyorum bu yüzden jsonserializer kullandık.
                    var categoryAjaxModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
                    {
                        CategoryDto=result.Data,
                        CategoryAddPartial= await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto),
                    });

                    return Json(categoryAjaxModel);
                }

                var categoryAjaxErrorModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
                {
                    CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto),
                });
                return Json(categoryAjaxErrorModel);
            }
        }

    }
}
