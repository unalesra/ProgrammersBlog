using ProgrammersBlog.Entities.Concreate;
using ProgrammersBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Entities.DTOs
{
    public class UserListDto:DtoGetBase
    {
        public IList<User> Users { get; set; }
    }
}
