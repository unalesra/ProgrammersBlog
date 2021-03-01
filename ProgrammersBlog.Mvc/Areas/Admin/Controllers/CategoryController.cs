using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Resuslts.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }


    }
}
