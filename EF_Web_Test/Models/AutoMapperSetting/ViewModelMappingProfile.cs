using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EF_Web_Test.Models.DTO;
namespace EF_Web_Test.Models.AutoMapperSetting
{
    public class ViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return GetType().Name;
            }
        }

        protected override void Configure()
       {
          
           //Mapper.CreateMap<Subject, SubjectDTO>().ForMember(dto => dto.CommentList, conf => conf.MapFrom(ol => ol.CommentList)); ;
           //Mapper.CreateMap<SubjectComment, SubjectCommentDTO>();
       }
    }
}