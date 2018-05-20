using System.Threading.Tasks;
using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorContosoUniversity.Client.Pages.Courses
{
    public class DetailsModel : BlazorComponent
    {
        [Parameter] string CourseID { get; set; }

        [Inject()] CoursesServiceClient Client { get; set; }

        public string CourseIDProp { get { return CourseID; } }

        public CourseDto Course { get; set; }

        protected override async Task OnInitAsync()
        {
            Course = await Client.GetDetails(CourseID);
        }

    }
}