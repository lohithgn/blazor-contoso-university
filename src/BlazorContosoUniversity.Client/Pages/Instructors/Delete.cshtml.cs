using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;
using System.Threading.Tasks;

namespace BlazorContosoUniversity.Client.Pages.Instructors
{
    public class DeleteModel : BlazorComponent
    {
        [Parameter] string InstructorID { get; set; }
        [Inject] InstructorsServiceClient Client { get; set; }
        [Inject] IUriHelper UriHelper { get; set; }

        public bool IsBusy { get; set; }
        public InstructorDto Instructor { get; set; }

        protected override async Task OnInitAsync()
        {
            IsBusy = true;
            await LoadInstructor(InstructorID);
            IsBusy = false;
        }

        private async Task LoadInstructor(string instructorID)
        {
            Instructor = await Client.GetDetails(instructorID);
        }

        public async Task OnDeleteClick()
        {
            var deleted = await Client.Delete(InstructorID);
            if (deleted)
            {
                UriHelper.NavigateTo("/instructors");
            }
        }

    }
}