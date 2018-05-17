using BlazorContosoUniversity.Client.Services;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlazorContosoUniversity.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(services =>
            {
                services.AddSingleton<StudentsServiceClient>();
                services.AddSingleton<CoursesServiceClient>();
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
