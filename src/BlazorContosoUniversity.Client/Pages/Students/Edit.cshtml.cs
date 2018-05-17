using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;
using System;
using System.Threading.Tasks;

namespace BlazorContosoUniversity.Client.Pages.Students
{
    public class EditModel : BlazorComponent
    {
        [Parameter]
        string StudentId { get; set; }
        [Inject()]
        StudentsServiceClient Client { get; set; }
        [Inject()]
        IUriHelper UriHelper { get; set; }

        public StudentDto Student { get; set; }
        public bool IsBusy { get; set; } = false;
        public string EnrollmentDateSting { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        protected override async Task OnInitAsync()
        {
            await LoadStudent();
        }
        public int LastDayInMonth = 31;
        private async Task LoadStudent()
        {
            IsBusy = true;
            StateHasChanged();
            Student = await Client.Get(StudentId);
            Day = Student.EnrollmentDate.Day;
            Month = Student.EnrollmentDate.Month;
            Year = Student.EnrollmentDate.Year;
            LastDayInMonth = Student.EnrollmentDate.AddMonths(1).AddDays(-Student.EnrollmentDate.Day).Day+1;
            IsBusy = false;
            StateHasChanged();
        }

        public async void OnSaveClick()
        {
            Student.EnrollmentDate = DateTime.Parse($"{Year}/{Month}/{Day}");
            var updated = await Client.Update(Student);
            UriHelper.NavigateTo("/students");
        }

        private bool IsLastNameProvided() => string.IsNullOrEmpty(Student.LastName.Trim());
        private bool IsLastNameLengthValid() => !string.IsNullOrEmpty(Student.LastName.Trim()) && Student.LastName.Length <= 50;
    }
}