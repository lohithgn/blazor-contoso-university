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
    [Route("api/departments")]
    public class DepartmentsController : Controller
    {
        readonly SchoolContext _context;
        readonly IMapper _mapper;

        public DepartmentsController(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/departments
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var departments = await _context.Departments
                                  .Include(a => a.Administrator)
                                  .AsNoTracking()
                                  .ToListAsync();
            var mapped = _mapper.Map<List<DepartmentDto>>(departments);
            return Ok(mapped);
        }

        // GET: api/departments/5
        [HttpGet]
        [Route("{id}"), ActionName("GetDepartment")]
        public async Task<IActionResult> GetById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                                       .Include(a => a.Administrator)
                                       .AsNoTracking()
                                       .SingleOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<DepartmentDto>(department));
        }

        // POST: api/departments
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromBody]DepartmentDto newDepartment)
        {
            if (newDepartment == null)
            {
                return BadRequest();
            }
            var departmentToCreate = new Department
            {
                Budget = newDepartment.Budget,
                Name = newDepartment.Name,
                StartDate = newDepartment.StartDate,
                InstructorID = newDepartment.InstructorID,
            };
            _context.Add(departmentToCreate);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetDepartment", new { id = departmentToCreate.DepartmentID });
        }

        // PUT: api/departments/5
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(int? id, [FromBody]DepartmentDto updatedDepartment)
        {
            if (updatedDepartment == null || updatedDepartment.DepartmentID != id)
            {
                return BadRequest();
            }

            var departmentToUpdate = await _context.Departments.FindAsync(id);
            if (departmentToUpdate == null)
            {
                return NotFound();
            }

            departmentToUpdate.Budget = updatedDepartment.Budget;
            departmentToUpdate.InstructorID = updatedDepartment.InstructorID;
            departmentToUpdate.Name = updatedDepartment.Name;
            departmentToUpdate.StartDate = updatedDepartment.StartDate;

            _context.Departments.Update(departmentToUpdate);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/departments/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
