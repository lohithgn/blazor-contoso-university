using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorContosoUniversity.Infrastructure;
using BlazorContosoUniversity.Models;
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
        [HttpGet]
        [Route("{id}"),ActionName("GetInstructor")]
        public async Task<IActionResult> GetDetails(int? id)
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
        public async Task<IActionResult> Post([FromBody]InstructorDto newInstructor) 
        {
            if(newInstructor == null)
            {
                return BadRequest();
            }

            var instructorToCreate = new Instructor();
            instructorToCreate.FirstMidName = newInstructor.FirstMidName;
            instructorToCreate.LastName = newInstructor.LastName;
            instructorToCreate.HireDate = newInstructor.HireDate;
            instructorToCreate.OfficeAssignment = null;
            if (!string.IsNullOrEmpty(newInstructor.Location))
            {
                instructorToCreate.OfficeAssignment = new OfficeAssignment { Location = newInstructor.Location };
            }
            instructorToCreate.CourseAssignments = new List<CourseAssignment>();
            foreach (var course in newInstructor.Courses)
            {
                var courseToAdd = new CourseAssignment { InstructorID = newInstructor.Id, CourseID = course.CourseID };
                instructorToCreate.CourseAssignments.Add(courseToAdd);
            }
            _context.Add(instructorToCreate);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetInstructor", new { id = newInstructor.Id });
        }

        // PUT: api/Instructors/5
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(int? id, [FromBody]InstructorDto updatedInstructor)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorToUpdate = await _context.Instructors
                                                    .Include(i => i.OfficeAssignment)
                                                    .Include(i => i.CourseAssignments)
                                                        .ThenInclude(i => i.Course)
                                                    .SingleOrDefaultAsync(m => m.ID == id);

            instructorToUpdate.FirstMidName = updatedInstructor.FirstMidName;
            instructorToUpdate.LastName = updatedInstructor.LastName;
            instructorToUpdate.HireDate = updatedInstructor.HireDate;
            if(!string.IsNullOrEmpty(updatedInstructor.Location))
            {
                instructorToUpdate.OfficeAssignment = new OfficeAssignment() { Location = updatedInstructor.Location };
            }
            else
            {
                instructorToUpdate.OfficeAssignment = null;
            }
            UpdateInstructorCourses(updatedInstructor.Courses.Select(i => i.CourseID.ToString()).ToArray(), instructorToUpdate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return NoContent();
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.CourseAssignments.Select(c => c.Course.CourseID));
            foreach (var course in _context.Courses)
            {
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.CourseAssignments.Add(new CourseAssignment { InstructorID = instructorToUpdate.ID, CourseID = course.CourseID });
                    }
                }
                else
                {

                    if (instructorCourses.Contains(course.CourseID))
                    {
                        CourseAssignment courseToRemove = instructorToUpdate.CourseAssignments.SingleOrDefault(i => i.CourseID == course.CourseID);
                        _context.Remove(courseToRemove);
                    }
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
