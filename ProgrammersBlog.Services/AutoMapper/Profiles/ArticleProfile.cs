using AutoMapper;
using ProgrammersBlog.Entities.Concreate;
using ProgrammersBlog.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Services.AutoMapper.Profiles
{
    public class ArticleProfile:Profile
    {
        public ArticleProfile()
        {
            //articledto'yu al article'a dönüştür demiş oldum
            CreateMap<ArticleAddDto, Article>().ForMember(dest=>dest.CreatedDate, opt=>opt.MapFrom(x=>DateTime.Now));
            CreateMap<ArticleUpdateDto, Article>().ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));

        }
    }
}
