using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;
using System.Threading.Tasks;

namespace BlazorContosoUniversity.Client.Pages.Students
{
    public class DeleteModel : BlazorComponent
    {
        [Parameter]
        string StudentId { get; set; }
        [Inject()]
        StudentsServiceClient Client { get; set; }
        [Inject()]
        IUriHelper UriHelper { get; set; }

        public StudentDto Student { get; set; } = null;

        protected override async Task OnInitAsync()
        {
            Student = await Client.Get(StudentId);
        }

        public async void OnDeleteClick()
        {
            var deleted = await Client.Delete(Student.Id);
            if(deleted)
            {
                UriHelper.NavigateTo("/students");
            }

        }
    }
}