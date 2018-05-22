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
    public class CreateModel : BlazorComponent
    {
        [Inject] IUriHelper UriHelper { get; set; }
        [Inject] InstructorsServiceClient InstructorClient { get; set; }
        [Inject] CoursesServiceClient CoursesClient { get; set; }

        public bool IsBusy { get; set; } = false;
        public InstructorDto Instructor { get; set; } = null;
        public List<CourseDto> Courses { get; set; }
        public List<AssignedCourseData> AssignedCourses;

        public int HireDay = DateTime.Today.Day;
        public int HireMonth = DateTime.Today.Month;
        public int HireYear = DateTime.Today.Year;
        public int LastDayInMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
        protected override async Task OnInitAsync()
        {
            IsBusy = true;
            await LoadCourses();
            LoadInstructor();
            IsBusy = false;
        }

        async Task LoadCourses()
        {
            Courses = await CoursesClient.Get();
        }

        void LoadInstructor()
        {
            Instructor = new InstructorDto();
            AssignedCourses = new List<AssignedCourseData>();
            foreach (var course in Courses)
            {
                AssignedCourses.Add(new AssignedCourseData
                {
                    CourseID = course.Id,
                    Title = course.Title,
                    Assigned = false
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
            var created = await InstructorClient.Create(Instructor);
            UriHelper.NavigateTo("/instructors");
        }
    }
}