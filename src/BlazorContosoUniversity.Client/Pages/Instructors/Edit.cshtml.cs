using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorContosoUniversity.Client.Models;
using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;

namespace BlazorContosoUniversity.Client.Pages.Instructors
{
    public class EditModel : BlazorComponent
    {
        [Parameter] string InstructorID { get; set; }
        [Inject] IUriHelper UriHelper { get; set;}
        [Inject] InstructorsServiceClient InstructorClient { get; set;}
        [Inject] CoursesServiceClient CoursesClient { get; set; }

        public bool IsBusy { get; set; } = false;
        public InstructorDto Instructor { get; set; } = null;
        public List<CourseDto> Courses { get; set; }
        public List<AssignedCourseData> AssignedCourses;

        public int HireDay = DateTime.Today.Day;
        public int HireMonth= DateTime.Today.Month;
        public int HireYear = DateTime.Today.Year;
        public int LastDayInMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
        protected override async Task OnInitAsync()
        {
            IsBusy = true;
            await LoadCourses();
            await LoadInstructor(InstructorID);
            IsBusy = false;
        }

        async Task LoadCourses()
        {
            Courses = await CoursesClient.Get();
        }

        async Task LoadInstructor(string instructorId)
        {
            Instructor = await InstructorClient.GetDetails(instructorId);
            HireDay = Instructor.HireDate.Day;
            HireMonth = Instructor.HireDate.Month;
            HireYear = Instructor.HireDate.Year;
            LastDayInMonth = DateTime.DaysInMonth(HireYear, HireMonth);

            var instructorCourses = new HashSet<int>(Instructor.Courses.Select(c => c.CourseID));
            AssignedCourses = new List<AssignedCourseData>();
            foreach (var course in Courses)
            {
                AssignedCourses.Add(new AssignedCourseData
                {
                    CourseID = course.Id,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.Id)
                });
            }

        }

        public async Task OnSaveClick()
        {
            Instructor.HireDate = DateTime.Parse($"{HireYear}/{HireMonth}/{HireDay}");
            Instructor.Courses = new List<CourseAssignmentDto>();
            foreach (var item in AssignedCourses.Where(c => c.Assigned))
            {
                Instructor.Courses.Add(new CourseAssignmentDto
                {
                    CourseID = item.CourseID,
                    CourseTitle = item.Title
                });
            }
            var updated = await InstructorClient.Update(Instructor);
            UriHelper.NavigateTo("/instructors");           
        }
    }
}