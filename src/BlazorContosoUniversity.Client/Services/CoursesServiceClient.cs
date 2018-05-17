using BlazorContosoUniversity.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;

namespace BlazorContosoUniversity.Client.Services
{
    public class CoursesServiceClient
    {
        string _baseUrl = "/api/courses";
        HttpClient _client;

        public CoursesServiceClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<CourseDto>> Get()
        {
            return await _client.GetJsonAsync<List<CourseDto>>(_baseUrl);
        }
    }
}
