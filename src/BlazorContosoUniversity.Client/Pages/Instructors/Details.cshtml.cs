using System;
using System.Threading.Tasks;
using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorContosoUniversity.Client.Pages.Instructors
{
    public class DetailsModel : BlazorComponent
    {
       [Parameter] string InstructorID { get; set; }
       [Inject] InstructorsServiceClient Client { get; set; }

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
    }
}