using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorContosoUniversity.Shared
{
    public class DepartmentDto
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public int Budget { get; set; }
        public string StartDate { get; set; }
        public string AdministratorName { get; set; }
    }
}
