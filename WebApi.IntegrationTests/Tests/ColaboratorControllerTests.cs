using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using Application.DTO;
using Application.Services;
using DataModel.Model;
using DataModel.Repository;
using Domain.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

using Xunit;

namespace WebApi.IntegrationTests.Tests
{
    public class ColaboratorControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ColaboratorControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true
            });
        }




        [Theory]
        [InlineData("/api/colaborator")]
        public async Task Post_EndpointReturnsBadRequestOnInvalidData(string url)
        {
            // arrange
            var client = _factory.CreateClient();
            var colaboratorDTO = new ColaboratorDTO
            {
                Name = "",
                Email = "invalid-email", 
                Street = "Test Street",
                PostalCode = "12345"
            };
            var jsonContent = JsonConvert.SerializeObject(colaboratorDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // act
            var response = await client.PostAsync(url, content);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/colaborator")]
        public async Task Post_EndpointResponseTimeIsAcceptable(string url)
        {
            // arrange
            var client = _factory.CreateClient();
            var colaboratorDTO = new ColaboratorDTO
            {
                Name = "Test Name",
                Email = "test@example.com",
                Street = "Test Street",
                PostalCode = "12345"
            };
            var jsonContent = JsonConvert.SerializeObject(colaboratorDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var maxResponseTime = TimeSpan.FromSeconds(3); 
            // act
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var response = await client.PostAsync(url, content);
            stopwatch.Stop();

            // assert
            response.EnsureSuccessStatusCode(); 
            Assert.True(stopwatch.Elapsed < maxResponseTime, $"Response time exceeded {maxResponseTime.TotalSeconds} seconds");
        }

        [Theory]
        [InlineData("/api/colaborator")]
        public async Task Post_EndpointReturnsBadRequestOnDuplicatedData(string url)
        {
            // arrange
            var client = _factory.CreateClient();
            var colaboratorDTO = new ColaboratorDTO
            {
                Name = "Duplicated Test",
                Email = "test@example.com",
                Street = "Test Street",
                PostalCode = "12345"
            };
            var jsonContent = JsonConvert.SerializeObject(colaboratorDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // act
            var response = await client.PostAsync(url, content);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        
        [Theory]
        [InlineData("/api/colaborator")]
        public async Task Post_EndpointReturnsSuccess(string url)
        {
            // arrange
            var client = _factory.CreateClient();

            
            var colaboratorDTO = new ColaboratorDTO
            {
                Name = "Test Name",
                Email = "tests@example.com",
                Street = "Test Street",
                PostalCode = "12345"
            };

          
            var jsonContent = JsonConvert.SerializeObject(colaboratorDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // act
            var response = await client.PostAsync(url, content);

            // assert
            response.EnsureSuccessStatusCode(); 
        }
    
    }


}