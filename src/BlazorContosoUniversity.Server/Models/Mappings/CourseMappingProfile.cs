using AutoMapper;
using BlazorContosoUniversity.Models;
using BlazorContosoUniversity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContosoUniversity.Server.Models
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<Course, CourseDto>().ForMember(m => m.Id, opt => opt.MapFrom(src => src.CourseID));
            CreateMap<CourseDto, Course>().ForMember(m => m.CourseID, opt => opt.MapFrom(src => src.Id));
        }
    }
}
