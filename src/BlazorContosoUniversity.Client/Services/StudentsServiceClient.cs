using BlazorContosoUniversity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;

namespace BlazorContosoUniversity.Client.Services
{
    public class StudentsServiceClient
    {
        HttpClient _client;
        string _baseUrl = "/api/students";
        public StudentsServiceClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<StudentDto>> Get(string searchString, string sortCol, string sortOrder)
        {
            var url = $"{_baseUrl}?query={searchString}&sortcol={sortCol}&sortorder={sortOrder}";
            return await _client.GetJsonAsync<List<StudentDto>>(url);
        }

        public async Task<StudentDto> Get(string StudentId)
        {
            var url = $"{_baseUrl}/{StudentId}";
            return await _client.GetJsonAsync<StudentDto>(url);
        }

        public async Task<StudentDetailsDto> GetDetails(string StudentId)
        {
            var url = $"{_baseUrl}/{StudentId}/details";
            return await _client.GetJsonAsync<StudentDetailsDto>(url);
        }

        public async Task<bool> Create(StudentDto student)
        {
            var url = $"{_baseUrl}";
            var created = false;
            try
            {
                await _client.PostJsonAsync<StudentDto>(url, student);
                created = true;
            }
            catch
            {
                created = false;
            }
            return created;
        }

        public async Task<bool> Update(StudentDto student)
        {
            var url = $"{_baseUrl}/{student.Id}";
            var updated = false;
            try
            {
                await _client.PutJsonAsync<StudentDetailsDto>(url,student);
                updated = true;
            }
            catch
            {
                updated = false;
            }
            return updated;
        }

        public async Task<bool> Delete(int id)
        {
            var url = $"{_baseUrl}/{id}";
            var deleted = false;
            try
            {
                await _client.DeleteAsync(url);
                deleted = true;
            }
            catch
            {
                deleted = false;
            }
            return deleted;
        }
    }
}
