using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorContosoUniversity.Client.Pages.Instructors
{
    public class IndexModel : BlazorComponent
    {
        [Inject] InstructorsServiceClient Client { get; set; }

        public List<InstructorDto> Instructors { get; set; } = null;
        public bool IsBusy { get; set; } = false;

        protected override async Task OnInitAsync()
        {
            IsBusy = true;
            await LoadCourses();
            IsBusy = false;
        }

        async Task LoadCourses()
        {
            Instructors = await Client.Get();
        }

    }
}