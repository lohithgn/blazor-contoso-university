using AutoMapper;
using BlazorContosoUniversity.Models;
using BlazorContosoUniversity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContosoUniversity.Server.Models
{
    public class InstructorMappingProfile : Profile
    {
        public InstructorMappingProfile()
        {
            CreateMap<Instructor, InstructorDto>()
                .ForMember(m => m.Id, opt => opt.MapFrom(src => src.ID))
                .ForMember(m => m.Location, opt => opt.MapFrom(src => src.OfficeAssignment.Location))
                .ForMember(m => m.Courses, opt => opt.MapFrom(src => src.CourseAssignments));
            CreateMap<CourseAssignment, CourseAssignmentDto>()
                .ForMember(m => m.DepartmentName, opt => opt.MapFrom(src => src.Course.Department.Name));
            CreateMap<Department, DepartmentDto>();
            //.ForMember(m => m.CourseID, opt => opt.MapFrom(src => src.Course.CourseID))
            //.ForMember(m => m.Title, opt => opt.MapFrom(src => src.Course.Title));
            CreateMap<InstructorDto, Instructor>().ForMember(m => m.ID, opt => opt.MapFrom(src => src.Id));
        }
    }
}
