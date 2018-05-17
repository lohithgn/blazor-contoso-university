using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorContosoUniversity.Infrastructure;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorContosoUniversity.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/courses")]
    public class CoursesController : Controller
    {
        readonly SchoolContext _context;
        readonly IMapper _mapper;

        public CoursesController(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var courses = await _context.Courses
                                  .Include(d => d.Department)
                                  .AsNoTracking()
                                  .ToListAsync();
            return Ok(_mapper.Map<List<CourseDto>>(courses));
        }

        // GET: api/Courses/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Courses
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
