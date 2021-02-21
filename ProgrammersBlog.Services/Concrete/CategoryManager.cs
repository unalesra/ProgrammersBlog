using AutoMapper;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Entities.Concreate;
using ProgrammersBlog.Entities.DTOs;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Resuslts.Abstract;
using ProgrammersBlog.Shared.Utilities.Resuslts.ComplexTypes;
using ProgrammersBlog.Shared.Utilities.Resuslts.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryManager(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<CategoryDto>> Add(CategoryAddDto categoryAddDto, string cretaedByName)
        {
            
            var category = _mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = cretaedByName;
            category.ModifiedByName = cretaedByName;

            // await _unitOfWork.Categories.AddAsync(category).ContinueWith(t => _unitOfWork.SaveAsync());
            //continueWith ile çok daha hızlı işlem yapılır, çünkü yaptığı task'den sonra hangisinin geleceğini biliyor oluyor
            //ama daha kayıt tamamlanmadan, kullanıcıya tamam kayıt oldu diye return etmiş oluyoruz dolayısıyla yönetimi zorlaşıyor
            //entity frammework core 5 db contex thread safe değildir.
            //db contex thread üzerinde çalışırken diğer thread beklesin diyor.
            //eğer yarı senkron yarı asenkron bir yapıda çalışıyor olsaydım bu bir sorun olmazdı, kimileri beklerken kimileri beklemezdi.
            //ama burada tamamen asenkron bir yapı var dolayısıyla aşağıdaki gibi kullamamız gerekir

            var addedCategory= await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync();

            return new DataResult<CategoryDto>(ResultStatus.Success, $"{categoryAddDto.Name} adlı kategori başarıyla işlenmiştir.", new CategoryDto { 
                Category=addedCategory,
                ResultStatus=ResultStatus.Success,
                Message= $"{categoryAddDto.Name} adlı kategori başarıyla işlenmiştir."
            });
        }

        public async Task<IResult> Delete(int categoryId, string modifiedByName)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);

            if (category != null)
            {
                category.IsDeleted = true;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;

                await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, $"{category.Name} adlı kategori başarıyla silindi.");
            }
            return new Result(ResultStatus.Error, $"{category.Name} adlı kategori bulunamadı.");
        }

        public async Task<IDataResult<CategoryDto>> Get(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);

            if (category != null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
                {
                    Category = category,
                    ResultStatus = ResultStatus.Success
                }) ;
               
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, "Böyle bir kategori bulunamadı", new CategoryDto {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = "Böyle bir kategori bulunamadı"
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(null, c => c.Articles);

            //hiç olmaya da bilir, 0 geldiğinde hata vermesin
            if (categories.Count > -1)
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto { 
                    Categories=categories,
                    ResultStatus=ResultStatus.Success
                });
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Hiçbir kategori bulunamadı", new CategoryListDto { 
                Categories=null,
                ResultStatus=ResultStatus.Error,
                Message= "Hiçbir kategori bulunamadı"
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeleted()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted, c => c.Articles);

            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto { 
                    Categories=categories,
                    ResultStatus=ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Hiçbir kategori bulunamadı", null);
        }

        public async Task<IResult> HardDelete(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);

            if (category != null)
            {
                await _unitOfWork.Categories.DeleteAsync(category);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, $"{category.Name} adlı kategori başarıyla veritabanından silindi.");
            }
            return new Result(ResultStatus.Error, $"{category.Name} adlı kategori bulunamadı.");
        }

        public async Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var category = _mapper.Map<Category>(categoryUpdateDto);
            category.ModifiedByName = modifiedByName;
            var updatedCategory= await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success, $"{categoryUpdateDto.Name} adlı kategori başarıyla güncellenmiştir.", new CategoryDto {
                Category = updatedCategory,
                ResultStatus = ResultStatus.Success,
                Message = $"{categoryUpdateDto.Name} adlı kategori başarıyla güncellenmiştir."
            });
        }
        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActive()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted&& c.IsActive, c => c.Articles);

            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Hiçbir kategori bulunamadı", null);
        }
    }
}
