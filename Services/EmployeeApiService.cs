using ERMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ERMS.Services
{
    public class EmployeeApiService
    {
        private readonly HttpClient _httpClient;
        public EmployeeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Retrieve all employees
        public async Task<List<Employee>> GetEmployeesAsync()
        {
            var response = await _httpClient.GetAsync("api/EmployeesApi");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Employee>>(json);
        }
        
        // GET: Retrieve a single employee by id
        public async Task<Employee> GetEmployeeAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/EmployeesApi/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Employee>(json);
        }

        // POST: Create a new employee
        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            // Serialize the employee object to JSON
            var jsonContent = JsonConvert.SerializeObject(employee);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send POST request
            var response = await _httpClient.PostAsync("api/EmployeesApi", content);
            response.EnsureSuccessStatusCode();

            // Deserialize the created employee from the response
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Employee>(jsonResponse);
        }

        // PUT: Update an existing employee
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            // Serialize the employee object to JSON
            var jsonContent = JsonConvert.SerializeObject(employee);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send PUT request using the employee's id in the URL
            var response = await _httpClient.PutAsync($"api/EmployeesApi/{employee.EmployeeId}", content);
            response.EnsureSuccessStatusCode();
        }

        // DELETE: Delete an employee by id
        public async Task DeleteEmployeeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/EmployeesApi/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
