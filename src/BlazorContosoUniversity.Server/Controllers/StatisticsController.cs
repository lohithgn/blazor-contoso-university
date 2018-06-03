using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using BlazorContosoUniversity.Infrastructure;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorContosoUniversity.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        readonly SchoolContext _context;
        public StatisticsController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var studentsCount = _context.Students.Count();
            var coursesCount = _context.Courses.Count();
            var instructorsCount = _context.Instructors.Count();
            var departmentsCount = _context.Departments.Count();

            return Ok(new StatisticsDto
                        {
                            Students = studentsCount,
                            Courses = coursesCount,
                            Instructors =instructorsCount,
                            Departments =departmentsCount
                    });
        }

        [HttpGet]
        [Route("enrollments")]
        public async Task<IActionResult> GetEnrollments()
        {
            List<EnrollmentDateGroupDto> groups = new List<EnrollmentDateGroupDto>();
            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
                        + "FROM Person "
                        + "WHERE Discriminator = 'Student' "
                        + "GROUP BY EnrollmentDate";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new EnrollmentDateGroupDto { EnrollmentDate = reader.GetDateTime(0), StudentCount = reader.GetInt32(1) };
                            groups.Add(row);
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }
            return Ok(groups);
        }
    }
}