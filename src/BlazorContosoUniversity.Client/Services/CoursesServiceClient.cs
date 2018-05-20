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

        public async Task<CourseDto> GetDetails(string id)
        {
            var url = $"{_baseUrl}/{id}";
            return await _client.GetJsonAsync<CourseDto>(url);
        }

        public async Task<bool> Create(CourseDto course)
        {
            var url = $"{_baseUrl}";
            var created = false;
            try
            {
                await _client.PostJsonAsync<CourseDto>(url, course);
                created = true;
            }
            catch
            {
                created = false;
            }
            return created;
        }

        public async Task<bool> Update(CourseDto course)
        {
            var url = $"{_baseUrl}/{course.Id}";
            var updated = false;
            try
            {
                await _client.PutJsonAsync<CourseDto>(url, course);
                updated = true;
            }
            catch
            {
                updated = false;
            }
            return updated;
        }

        public async Task<bool> Delete(string id)
        {
            var url = $"{_baseUrl}/{id}";
            var deleted = false;
            try
            {
                await _client.DeleteAsync(url);
                deleted = true;
            }
            catch { deleted = false; }
            return deleted;
        }
    }
}
