using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorContosoUniversity.Infrastructure;
using BlazorContosoUniversity.Models;
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
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var courses = await _context.Courses
                                  .Include(d => d.Department)
                                  .AsNoTracking()
                                  .ToListAsync();
            return Ok(_mapper.Map<List<CourseDto>>(courses));
        }

        // GET: api/Courses/5
        [HttpGet, ActionName("GetCourse")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                                       .Include(c => c.Department)
                                       .AsNoTracking()
                                       .SingleOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CourseDto>(course));
        }

        // POST: api/Courses
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromBody] CourseDto course)
        {
            if (course == null)
            {
                return BadRequest();
            }
            var newCourse = _mapper.Map<Course>(course);
            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetCourse", new { id = newCourse.CourseID }, newCourse);
        }

        // PUT: api/Courses/5
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(int? id, [FromBody]CourseDto courseToEdit)
        {
            if (courseToEdit == null || courseToEdit.Id != id)
            {
                return BadRequest();
            }

            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            course.Title = courseToEdit.Title;
            course.Credits = courseToEdit.Credits;
            course.DepartmentID = courseToEdit.DepartmentID;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
