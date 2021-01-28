using ProgrammersBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Entities.Concreate
{
    public class User:EntityBase,IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }//şifrelenerek tutulacağı için bu şekilde tuttum
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public ICollection<Article> Articles { get; set; }

    }
}
