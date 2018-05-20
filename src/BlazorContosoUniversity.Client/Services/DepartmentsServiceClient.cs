using BlazorContosoUniversity.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;

namespace BlazorContosoUniversity.Client.Services
{
    public class DepartmentsServiceClient
    {
        string _baseUrl = "/api/departments";
        HttpClient _client;

        public DepartmentsServiceClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<DepartmentDto>> Get()
        {
            return await _client.GetJsonAsync<List<DepartmentDto>>(_baseUrl);
        }

        public async Task<CourseDto> GetDetails(string id)
        {
            var url = $"{_baseUrl}/{id}";
            return await _client.GetJsonAsync<CourseDto>(url);
        }
    }
}
