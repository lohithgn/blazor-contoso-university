using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorContosoUniversity.Shared
{
    public class StudentDto
    {
        public StudentDto(){}

        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }

        public string FullName {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }
        public DateTime EnrollmentDate { get; set; }
    }
}
