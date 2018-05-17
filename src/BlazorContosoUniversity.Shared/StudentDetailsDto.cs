using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorContosoUniversity.Shared
{
    public class StudentDetailsDto : StudentDto
    {
        public List<EnrollmentDto> Enrollments { get; set; }
    }
}
