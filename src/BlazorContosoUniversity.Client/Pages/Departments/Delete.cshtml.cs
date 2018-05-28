using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;
using System.Threading.Tasks;

namespace BlazorContosoUniversity.Client.Pages.Departments
{
    public class DeleteModel : BlazorComponent
    {
        [Inject] IUriHelper UriHelper { get; set; }
        [Parameter] string DepartmentID { get; set; }
        [Inject] DepartmentsServiceClient Client { get; set; }


        public bool IsBusy { get; set; }
        public DepartmentDto Department;

        protected override async Task OnInitAsync()
        {
            IsBusy = true;
            await LoadDepartment(DepartmentID);
            IsBusy = false;
        }

        async Task LoadDepartment(string deptId)
        {
            Department = await Client.GetDetails(deptId);
        }

        public async Task OnDeleteClick()
        {
            var deleted = await Client.Delete(DepartmentID);
            UriHelper.NavigateTo("/departments");
        }
    }
}