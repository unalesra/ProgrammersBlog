using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProgrammersBlog.Entities.DTOs;
using ProgrammersBlog.Shared.Utilities.Resuslts.Abstract;

namespace ProgrammersBlog.Mvc.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<ImageUploadedDto>> UploadUserImage(string userName, IFormFile pictureFile, string folderName = "userImages");
        IDataResult<ImageDeletedDto> Delete(string pictureName);
    }
}
