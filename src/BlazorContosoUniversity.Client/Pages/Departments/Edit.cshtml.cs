using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorContosoUniversity.Client.Pages.Departments
{
    public class EditModel : BlazorComponent
    {
        [Inject] IUriHelper UriHelper { get; set; }
        [Parameter] string DepartmentID { get; set; }
        [Inject] InstructorsServiceClient InstructorsClient { get; set; }
        [Inject] DepartmentsServiceClient DepartmentsClient { get; set; }

        public bool IsBusy { get; set; } = false;
        public List<InstructorDto> Instructors { get; set; }
        public DepartmentDto Department { get; set; }

        public int StartDay = DateTime.Today.Day;
        public int StartMonth = DateTime.Today.Month;
        public int StartYear = DateTime.Today.Year;
        public int LastDayInMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);

        protected override async Task OnInitAsync()
        {
            IsBusy = true;
            await LoadDepartment();
            IsBusy = false;
        }

        async Task LoadDepartment()
        {
            var instructorsTask = InstructorsClient.Get();
            var departmentTask = DepartmentsClient.GetDetails(DepartmentID);

            Instructors = await instructorsTask;
            Department = await departmentTask;

            StartDay = Department.StartDate.Day;
            StartMonth = Department.StartDate.Month;
            StartYear = Department.StartDate.Year;
            LastDayInMonth = DateTime.DaysInMonth(StartYear, StartMonth);
        }

        public async void OnSaveClick()
        {
            Department.StartDate = DateTime.Parse($"{StartYear}/{StartMonth}/{StartDay}");
            var updated = await DepartmentsClient.Update(Department);
            UriHelper.NavigateTo("/departments");
        }
    }
}