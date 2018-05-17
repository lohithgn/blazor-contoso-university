using System.Net.Http;
using System.Threading.Tasks;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor;
using BlazorContosoUniversity.Client.Services;

namespace BlazorContosoUniversity.Client.Pages.Students
{
    public class DetailsModel : BlazorComponent
    {
        [Parameter]
        string StudentId { get; set; }

        [Inject()]
        StudentsServiceClient Client { get; set; }

        public StudentDetailsDto Student { get; set; } = null;

        protected override async Task OnInitAsync()
        {
            Student = await Client.GetDetails(StudentId);
            StateHasChanged();
        }
    }
}