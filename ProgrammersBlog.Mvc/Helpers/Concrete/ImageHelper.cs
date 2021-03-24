using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProgrammersBlog.Entities.DTOs;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using ProgrammersBlog.Shared.Utilities.Extensions;
using ProgrammersBlog.Shared.Utilities.Resuslts.Abstract;
using ProgrammersBlog.Shared.Utilities.Resuslts.ComplexTypes;
using ProgrammersBlog.Shared.Utilities.Resuslts.Concrete;

namespace ProgrammersBlog.Mvc.Helpers.Concrete
{
    public class ImageHelper:IImageHelper
    {

        //wwwroot'un yolu değişse bile hata almamakk için dinamik olarak ben yolu alamaya devam edebilmek için kullanıyorum. (img ekleme işlemi için lazım)
        private readonly IWebHostEnvironment _env;
        private readonly string _wwwroot;
        private readonly string imgFolder = "img";

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwroot = _env.WebRootPath;
        }

        public async Task<IDataResult<ImageUploadedDto>> UploadUserImage(string userName, IFormFile pictureFile, string folderName="userImages")
        {
            //eğer kaydetmek istediğim klasör yoksa 
            if (!Directory.Exists($"{_wwwroot}/{imgFolder}/{folderName}"))
            {
                Directory.CreateDirectory($"{_wwwroot}/{imgFolder}/{folderName}");
            }


            string oldFileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);
            string fileExtension = Path.GetExtension(pictureFile.FileName);
            DateTime dateTime = DateTime.Now;
            // AlperTunga_587_5_38_12_3_10_2020.png
            string newFileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
            var path = Path.Combine($"{_wwwroot}/{imgFolder}/{folderName}", newFileName);
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }

            return new DataResult<ImageUploadedDto>(ResultStatus.Success,$"{userName} adlı kullanıcının resimi başarıyla yüklenmiştir.",new ImageUploadedDto
            {
                FullName = $"{folderName}/{newFileName}",
                OldName = oldFileName,
                Extension = fileExtension,
                FolderName = folderName,
                Path = path,
                Size = pictureFile.Length
            });
        }

        public IDataResult<ImageDeletedDto> Delete(string pictureName)
        {
            var fileToDelete = Path.Combine($"{_wwwroot}/{imgFolder}/", pictureName);
            if (System.IO.File.Exists(fileToDelete))
            {
                var fileInfo = new FileInfo(fileToDelete);
                var imageDeletedDto = new ImageDeletedDto
                {
                    FullName = pictureName,
                    Extension = fileInfo.Extension,
                    Path = fileInfo.FullName,
                    Size = fileInfo.Length
                };
                System.IO.File.Delete(fileToDelete);
                return new DataResult<ImageDeletedDto>(ResultStatus.Success,imageDeletedDto);
            }
            else
            {
                return new DataResult<ImageDeletedDto>(ResultStatus.Error,$"Böyle bir resim bulunamadı.",null);
            }
        }
        
    }
}
