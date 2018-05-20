using AutoMapper;
using BlazorContosoUniversity.Models;
using BlazorContosoUniversity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContosoUniversity.Server.Models
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<Department, DepartmentDto>();
        }
    }
}
