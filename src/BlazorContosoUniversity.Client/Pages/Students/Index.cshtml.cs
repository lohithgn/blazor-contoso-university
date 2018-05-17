using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor;
using BlazorContosoUniversity.Client.Services;

namespace BlazorContosoUniversity.Client.Pages.Students
{
    public class IndexModel : BlazorComponent
    {
        [Inject()]  
        StudentsServiceClient Client { get; set; }
        string sortColumn = "name";
        string sortOrder = "asc";

        public List<StudentDto> Students { get; set; } = null;
        public string SearchString { get; set; } = "";

        protected override async Task OnInitAsync()
        {
            await LoadStudents();
        } 

        protected async void SortByLastName()
        {
            SetSortOptions("name");
            await LoadStudents();
        }

        protected async void SortByEnrollmentDate()
        {
            SetSortOptions("date");
            await LoadStudents();
        }

        protected async void OnSearchClick()
        {
            await LoadStudents();
        }

        private async Task LoadStudents()
        {
            Students = await Client.Get(SearchString, sortColumn, sortOrder);
            StateHasChanged();
        }

        private void SetSortOptions(string currentSortcolumn)
        {
            if (sortColumn != currentSortcolumn)
            {
                sortOrder = "asc";
            }
            else
            {
                sortOrder = sortOrder == "asc" ? "desc" : "asc";
            }
            sortColumn = currentSortcolumn;
        }


        public string NameSortClass
        {
            get
            {
                var dirClass = sortOrder == "desc" ? "oi-arrow-bottom" : "oi-arrow-top";
                var visibleClass = sortColumn == "name" ? "visible" : "invisible";
                return $"oi {dirClass} {visibleClass}";
            }
        }

        public string DateSortClass
        {
            get
            {
                var dirClass = sortOrder == "desc" ? "oi-arrow-bottom" : "oi-arrow-top";
                var visibleClass = sortColumn == "date" ? "visible" : "invisible";
                return $"oi {dirClass} {visibleClass}";
            }
        }


    }
}