using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using TestHTTPClient.DTOs;
using TestHTTPClient.EntitiesModel;
using TestHTTPClient.ResponseModel;

namespace TestHTTPClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpClientFroCustomer : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<HttpClientFroCustomer> _logger;

        public HttpClientFroCustomer(ILogger<HttpClientFroCustomer> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetCustomer")]
        public async Task<CustomerResponse<List<Customer>>> Get()
        {
            CustomerResponse<List<Customer>> responsemodel = new CustomerResponse<List<Customer>>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7042/api/Customer");
                    var response = await client.GetAsync(client.BaseAddress);
                    var dataone = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<CustomerResponse<List<Customer>>>(dataone);

                    return data;
                }
            }
            catch
            {
                return responsemodel;
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CustomerAddDTO cust)
        {
            
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7042/api/Customer/Add");
                    var data = JsonConvert.SerializeObject(cust);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("Customer added successsfully");
                    }
                    else
                    {
                        return BadRequest("Failed to create Customer");
                    }
                }
            }
            catch
            {
                return BadRequest("Failed to add new Customer");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Customer cust)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7042/api/Customer/Put/{Id}");
                    var data = JsonConvert.SerializeObject(cust);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(client.BaseAddress, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("Customer edited successsfully");
                    }
                    else
                    {
                        return BadRequest("Failed edit Customer");
                    }
                }
            }
            catch
            {
                return BadRequest("Failed to edit Customer");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"https://localhost:7042/api/Customer/Delete/{id}");
                    var response = await client.DeleteAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("Customer successfully deleted");
                    }
                    else
                    {
                        return BadRequest($"Failed to delete customer with {id}");
                    }
                }
            }
            catch 
            {
                return BadRequest($"Failed to delete customer with {id}");
            }
        }
    }
}