using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorContosoUniversity.Shared
{
    public class InstructorDto
    {
        public InstructorDto(){}   
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime HireDate { get; set; }
        public string Location { get; set; }
        public List<CourseAssignmentDto> Courses { get; set; }
    }
}
