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
            return Ok(_mapper.Map<List<DepartmentDto>>(departments));
        }

        // GET: api/departments/5
        [HttpGet, ActionName("GetDepartment")]
        [Route("{id}")]
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
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/departments/5
        [HttpPut]
        [Route("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/departments/5
        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
        }
    }
}
