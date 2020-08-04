using Application.DTOs;
using Application.POCOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebApi.IntegrationTests.Controllers
{
    public class ProductControllerIntegrationTests : IClassFixture<TestApplicationFactory>
    {
        private readonly TestApplicationFactory _factory;

        public ProductControllerIntegrationTests(TestApplicationFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/Product")]
        public async Task GetAllProducts_Unauthorized(string url)
        {
            // Arrange
            var provider = TestClaimsProvider.WithAnonymousUserClaims();
            var client = _factory.CreateClientWithTestAuth(provider);

            // Act
            var response = await client.GetAsync(url);
            // Assert 1
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/Product")]
        public async Task GetAllProducts_Authorized(string url)
        {
            // Arrange
            var provider = TestClaimsProvider.WithBasicUserClaims();
            var client = _factory.CreateClientWithTestAuth(provider);

            // Act
            var response = await client.GetAsync(url);
            // Assert 1
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Assert 2
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            // Deserialize and examine results.
            var stringResponse = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductDto>>(stringResponse);

            // Assert 3
            Assert.True(products.Count >= 2);
        }

        [Theory]
        [InlineData("/api/Product/2")]
        public async Task GetProduct_Valid_OK(string url)
        {
            // Arrange
            var provider = TestClaimsProvider.WithAnonymousUserClaims();
            var client = _factory.CreateClientWithTestAuth(provider);

            // Act
            var response = await client.GetAsync(url);
            // Assert 1
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Assert 2
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            // Deserialize and examine results.
            var stringResponse = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ProductDto>(stringResponse);

            // Assert 3
            Assert.Equal(2, product.Id);
        }

        [Theory]
        [InlineData("/api/Product/0")]
        public async Task GetProduct_InValid_NotFound(string url)
        {
            // Arrange
            var provider = TestClaimsProvider.WithAnonymousUserClaims();
            var client = _factory.CreateClientWithTestAuth(provider);

            // Act
            var response = await client.GetAsync(url);
            // Assert 1
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/Product")]
        public async Task PostProduct_Valid_OK(string url)
        {
            // Arrange
            var provider = TestClaimsProvider.WithAdminUserClaims();
            var client = _factory.CreateClientWithTestAuth(provider);

            var product = new ProductPoco
            {
                Data = new ProductDto
                {
                    Name = "Nuraphone",
                    Description = "The nuraphone is the only headphone to automatically learn and adapt to your unique hearing. Listen to Music in Full Colour™ with personalized sound.",
                    Price = 400
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(url, content);
            // Assert 1
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            // Deserialize and examine results.
            var stringResponse = await response.Content.ReadAsStringAsync();
            var newProduct = JsonConvert.DeserializeObject<ProductDto>(stringResponse);

            // Assert 3
            Assert.NotEqual(0, newProduct.Id);

            // Assert 4
            Assert.Equal("Nuraphone", newProduct.Name);
        }

        [Theory]
        [InlineData("/api/Product")]
        public async Task PutProduct_Valid_OK(string url)
        {
            // Arrange
            var provider = TestClaimsProvider.WithAdminUserClaims();
            var client = _factory.CreateClientWithTestAuth(provider);

            var product = new ProductDto
            {
                Id = 1,
                Name = "iPhone XR",
                Price = 444
            };

            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync(url, content);

            // Assert 1
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/Product/2")]
        public async Task DeleteProduct_Valid_OK(string url)
        {
            // Arrange
            var provider = TestClaimsProvider.WithAdminUserClaims();
            var client = _factory.CreateClientWithTestAuth(provider);

            // Act
            var responseDelete = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, responseDelete.StatusCode);
        }
    }
}
