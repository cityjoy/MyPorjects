using AutoMapper;
using EF_Web_Test.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EF_Web_Test.Models.Entity;

namespace EF_Web_Test.Models.AutoMapperSetting
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Subject, SubjectDTO>().ForMember(s=>s.CommentList,map=>map.MapFrom(s=>s.CommentList));
                cfg.CreateMap<SubjectComment, SubjectCommentDTO>();
                cfg.AddProfile<ViewModelMappingProfile>();//添加一个配置文件
            });
            Mapper.AssertConfigurationIsValid();
        }
    }
}