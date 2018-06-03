using System;
using System.Threading.Tasks;
using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;

namespace BlazorContosoUniversity.Client.Pages.Students
{
    public class CreateModel : BlazorComponent
    {
        [Inject()]
        StudentsServiceClient Client { get; set; }
        [Inject()]
        IUriHelper UriHelper { get; set; }

        public StudentDto Student;
        public int Day;
        public int Month;
        public int Year;
        public int LastDayInMonth;

        protected override void OnInit()
        {
            Student = new StudentDto()
            {
                EnrollmentDate = DateTime.Today
            };
            Day = DateTime.Today.Day;
            Month = DateTime.Today.Month;
            Year = DateTime.Today.Year;
            LastDayInMonth = DateTime.DaysInMonth(Year, Month);
        }

        public async Task OnSaveClick()
        {
            Student.EnrollmentDate = DateTime.Parse($"{Year}/{Month}/{Day}");
            var created = await Client.Create(Student);
            UriHelper.NavigateTo("/students");
        }
    }
}