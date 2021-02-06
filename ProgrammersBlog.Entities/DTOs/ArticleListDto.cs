using ProgrammersBlog.Entities.Concreate;
using ProgrammersBlog.Shared.Entities.Abstract;
using ProgrammersBlog.Shared.Utilities.Resuslts.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Entities.DTOs
{
    public class ArticleListDto: DtoGetBase
    {
        public IList<Article> Article { get; set; }
    }
}
