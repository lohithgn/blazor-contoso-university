using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorContosoUniversity.Infrastructure;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorContosoUniversity.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class InstructorsController : Controller
    {
        readonly SchoolContext _context;
        readonly IMapper _mapper;
        public InstructorsController(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Instructors
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var instructors = await _context.Instructors
                                      .Include(o => o.OfficeAssignment)
                                      .Include(c => c.CourseAssignments)
                                        .ThenInclude(c => c.Course)
                                      .OrderBy(l => l.LastName)
                                      .AsNoTracking()
                                      .ToListAsync();
            var mapped = _mapper.Map<List<InstructorDto>>(instructors);
            return Ok(mapped);
        }

        // GET: api/Instructors/5
        [HttpGet("{id}", Name = "GetInstructor")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                                      .Include(o => o.OfficeAssignment)
                                      .Include(c => c.CourseAssignments)
                                        .ThenInclude(c => c.Course)
                                            .ThenInclude(d => d.Department)
                                      .AsNoTracking()
                                      .SingleOrDefaultAsync(i => i.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            var mapped = _mapper.Map<InstructorDto>(instructor);
            return Ok(mapped);
        }

        // POST: api/Instructors
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Instructors/5
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
