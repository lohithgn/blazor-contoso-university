using AutoMapper;
using BlazorContosoUniversity.Infrastructure;
using BlazorContosoUniversity.Models;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorContosoUniversity.Controllers
{
    [Route("api/students")]
    public class StudentsController : Controller
    {
        readonly SchoolContext _context;
        readonly IMapper _mapper;

        public StudentsController(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/students?sortCol=&sortOrder=&query=
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get([FromQuery] string sortCol = "name",
                                             [FromQuery] string sortOrder = "asc",
                                             [FromQuery] string query = "")
        {
            var studentsQuery = _context.Students.Select(s => s);

            if (!String.IsNullOrEmpty(query))
            {
                studentsQuery = studentsQuery.Where(s => s.LastName.Contains(query)
                                                    || s.FirstMidName.Contains(query));
            }

            switch ($"{sortCol}_{sortOrder}")
            {
                
                case "name_desc":
                    studentsQuery = studentsQuery.OrderByDescending(s => s.LastName);
                    break;
                case "date_asc":
                    studentsQuery = studentsQuery.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentsQuery = studentsQuery.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentsQuery = studentsQuery.OrderBy(s => s.LastName);
                    break;
            }

            var students = await studentsQuery.AsNoTracking().ToListAsync();

            var studentsList = _mapper.Map<List<StudentDto>>(students);
            return Ok(studentsList);
        }

        // GET: api/students/<id>/details
        [HttpGet]
        [Route("{id}/details")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var student = await _context.Students
                                         .Include(s => s.Enrollments)
                                            .ThenInclude(e => e.Course)
                                         .AsNoTracking()
                                         .SingleOrDefaultAsync(m => m.ID == id);
            var studentDto = _mapper.Map<StudentDetailsDto>(student);
            return Ok(studentDto);
        }

        // GET: api/students/<id>
        [HttpGet, ActionName("GetStudent")]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var student = await _context.Students
                                        .AsNoTracking()
                                        .SingleOrDefaultAsync(m => m.ID == id);
            var studentDto = _mapper.Map<StudentDto>(student);
            return Ok(studentDto);
        }

        [HttpPost]
        public IActionResult Post([FromBody] StudentDto student)
        {
            if (student == null)
            {
                return BadRequest();
            }
            var newStudent = _mapper.Map<Student>(student);
            _context.Students.Add(newStudent);
            _context.SaveChanges();

            return CreatedAtRoute("GetStudent", new { id = newStudent.ID }, newStudent);
        }


        // GET: api/students/<id>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StudentDto studentToEdit)
        {
            if (studentToEdit == null || studentToEdit.Id != id)
            {
                return BadRequest();
            }

            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            student.LastName = studentToEdit.LastName;
            student.FirstMidName = studentToEdit.FirstMidName;
            student.EnrollmentDate = studentToEdit.EnrollmentDate;

            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
