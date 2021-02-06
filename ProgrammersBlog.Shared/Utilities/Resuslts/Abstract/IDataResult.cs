using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Shared.Utilities.Resuslts.Abstract
{
    //out T ---> kategoriyi tek taşımak da isteyebilirim, liste olarak da taşımak isteyebilirim.
    //bu sayede nasıl istersem öyle taşıyabilmiş oldum
    public interface IDataResult<out T>:IResult
    {
        public T Data { get; } //data içine istersem bir kategori atabilirim, istersem bir kategori list de atabilirim
        // new DataResult<Category>(ResultStatus.Success, category)
        // new DataResult<IList<Category>>(ResultStatus.Success, categoryList)
        // new DataResult<IEnumerable<Category>>(ResultStatus.Success, categoryList)


    }
}
