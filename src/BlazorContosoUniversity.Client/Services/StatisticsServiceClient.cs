using BlazorContosoUniversity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;

namespace BlazorContosoUniversity.Client.Services
{
    public class StatisticsServiceClient
    {
        HttpClient _client;
        string _baseUrl = "/api/statistics";
        public StatisticsServiceClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<StatisticsDto> Get()
        {
            return await _client.GetJsonAsync<StatisticsDto>(_baseUrl);
        }

        public async Task<List<EnrollmentDateGroupDto>> GetEnrollmentStats()
        {
            var url = $"{_baseUrl}/enrollments";
            return await _client.GetJsonAsync<List<EnrollmentDateGroupDto>>(url);
        }
    }
}
