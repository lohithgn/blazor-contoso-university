using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorContosoUniversity.Client.Services;
using BlazorContosoUniversity.Shared;
using Microsoft.AspNetCore.Blazor.Browser.Interop;
using Microsoft.AspNetCore.Blazor.Components;
using System.Linq;

namespace BlazorContosoUniversity.Client.Pages
{
    public class AboutModel : BlazorComponent
    {
        [Inject] StatisticsServiceClient Client { get; set; }

        public bool IsBusy { get; set; }

        public StatisticsDto Stats { get; set; } = null;

        List<EnrollmentDateGroupDto> enrollmentsData = new List<EnrollmentDateGroupDto>();

        protected override async Task OnInitAsync()
        {
            IsBusy = true;
            await LoadStats();
            await LoadEnrollmentStats();
            IsBusy = false;
        }

        private async Task LoadEnrollmentStats()
        {
            enrollmentsData = await Client.GetEnrollmentStats();
        }

        protected override void OnAfterRender()
        {
            RegisteredFunction.Invoke<bool>("drawEnrollmentStatsChart", enrollmentsData);
        }

        async Task LoadStats()
        {
            Stats = await Client.Get();
        }
    }
}