using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorContosoUniversity.Client.Pages.Departments
{
    public class CreateModel : BlazorComponent
    {
        [Inject] IUriHelper UriHelper { get; set; }
        [Inject] InstructorsServiceClient InstructorsClient { get; set; }
        [Inject] DepartmentsServiceClient DepartmentsClient { get; set; }

        public bool IsBusy { get; set; } = false;
        public List<InstructorDto> Instructors { get; set; }
        public DepartmentDto Department { get; set; } = new DepartmentDto();

        public int StartDay = DateTime.Today.Day;
        public int StartMonth = DateTime.Today.Month;
        public int StartYear = DateTime.Today.Year;
        public int LastDayInMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);

        protected override async Task OnInitAsync()
        {
            IsBusy = true;
            await LoadInstructors();
            IsBusy = false;
        }

        async Task LoadInstructors()
        {
            var instructorsTask = InstructorsClient.Get();
            Instructors = await instructorsTask;
        }

        public async void OnSaveClick()
        {
            Department.StartDate = DateTime.Parse($"{StartYear}/{StartMonth}/{StartDay}");
            var updated = await DepartmentsClient.Create(Department);
            UriHelper.NavigateTo("/departments");
        }
    }
}