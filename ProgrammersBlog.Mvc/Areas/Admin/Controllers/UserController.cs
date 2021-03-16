using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Entities.Concreate;
using ProgrammersBlog.Entities.DTOs;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Shared.Utilities.Extensions;
using ProgrammersBlog.Shared.Utilities.Resuslts.ComplexTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {

        //userservice gibi düşün bunu Identity'den geliyor
        private readonly UserManager<User> _userManager;

        //wwwroot'un yolu değişse bile hata almamakk için dinamik olarak ben yolu alamaya devam edebilmek için kullanıyorum. (img ekleme işlemi için lazım)
        private readonly IWebHostEnvironment _env;

        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IWebHostEnvironment env, IMapper mapper)
        {
            _userManager = userManager;
            _env = env;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(new UserListDto
            {
                Users = users,
                ResultStatus=ResultStatus.Success
            });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }


        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                userAddDto.Picture = await ImageUpload(userAddDto);
                var user = _mapper.Map<User>(userAddDto);

                //şifreyi hashlediği için ayrı olarak alıyor.
                var result = await _userManager.CreateAsync(user, userAddDto.Password);

                //Identity tarafında hata alıp almadığımızı da kontrol etmemiz gerekiyor.
                if (result.Succeeded)
                {
                    var userAddAjaxModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{user.UserName} adlı kullanıcı adına sahip, kullanıcı başarıyla eklenmiştir.",
                            User = user
                        },
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    //hatayı önyüze gönderebilmek için tekrar useraddajaxmodel lazım

                    var userAddAjaxModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserAddDto=userAddDto,
                        UserAddPartial=await this.RenderViewToStringAsync("_UserAddPartial", userAddDto),
                    });

                    return Json(userAddAjaxModel);
                }
            }

            var userAddAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
            {
                UserAddDto = userAddDto,
                UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto),
            });

            return Json(userAddAjaxModelStateErrorModel);
        }

        public async Task<String> ImageUpload(UserAddDto userAddDto)
        {
            string wwwroot = _env.WebRootPath;  //wwwroot'un dosya yolunu dinamik bir şekilde aldım.

            //    string fileName = Path.GetFileNameWithoutExtension(userAddDto.PictureFile.FileName); //sondaki uzantıyı sildik. .png gibi
            string fileExtension = Path.GetExtension(userAddDto.PictureFile.FileName); //.png'yi aldık

            DateTime dateTime = DateTime.Now;
            string fileName = $"{userAddDto.UserName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
            var path = Path.Combine($"{wwwroot}/img",fileName);


            //Filestream dosyalarla ilgli işlemleri yöneten sınıf
            await using (var stream= new FileStream(path, FileMode.Create))
            {
                await userAddDto.PictureFile.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
