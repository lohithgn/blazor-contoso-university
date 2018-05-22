using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorContosoUniversity.Client.Pages.Departments
{
    public class IndexModel : BlazorComponent
    {
        [Inject] DepartmentsServiceClient Client { get; set; }

        public bool IsBusy { get; set; }
        public List<DepartmentDto> Departments;

        protected override async Task OnInitAsync()
        {
            IsBusy = true;
            await LoadDepartments();
            IsBusy = false;
        }

        async Task LoadDepartments()
        {
            Departments = await Client.Get();
        }
    }
}