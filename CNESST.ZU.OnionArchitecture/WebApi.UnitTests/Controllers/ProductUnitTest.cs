using Application.DTOs;
using Application.Interfaces.Services;
using Application.POCOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Controllers;
using Xunit;

namespace WebApi.UnitTests.Controllers
{
    public class ProductUnitTest
    {
        [Fact]
        public async Task GetOne_WhenCalledWithId_ReturnProductModelResult_Async()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductService = new Mock<IProductService>();


            ProductDto iPhoneXr = new ProductDto
            {
                Id = 219,
                Name = "iPhone XR",
                Description = "Description for iPhone XR",
                Price = 470
            };

            mockProductService
                .Setup(r => r.GetProduct(219)).Returns(Task.FromResult(iPhoneXr));

            var controller = new ProductController(
                mockLogger.Object,
                mockProductService.Object);

            // Act
            var actionResult = await controller.GetOneAsync(219);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsAssignableFrom<ActionResult<ProductDto>>(actionResult);

            Assert.Equal(iPhoneXr, actionResult.Value);
        }

        [Fact]
        public async Task GetOne_WhenCalledWithWrongId_ReturnNotFoundResult_Async()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductService = new Mock<IProductService>();

            mockProductService
                .Setup(r => r.GetProduct(219)).Returns(Task.FromResult(null as ProductDto));

            var controller = new ProductController(
                mockLogger.Object,
                mockProductService.Object);

            // Act
            var actionResult = await controller.GetOneAsync(0);

            // Assert
            Assert.NotNull(actionResult);

            Assert.Equal(StatusCodes.Status404NotFound, (actionResult.Result as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task GetOne_ThrowException_Async()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductService = new Mock<IProductService>();

            mockProductService
                .Setup(r => r.GetProduct(1)).Throws(new Exception("..."));

            var controller = new ProductController(
                mockLogger.Object,
                mockProductService.Object);

            // Act
            var result = await controller.GetOneAsync(1);

            // Assert
            Assert.NotNull(result);

            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetAll_ReturnListProductModelResult_Async()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductService = new Mock<IProductService>();

            IList<ProductDto> phones = new List<ProductDto>();
            phones.Add(new ProductDto { Id = 1, Name = "phone n° 1" });
            phones.Add(new ProductDto { Id = 2, Name = "phone n° 2" });
            phones.Add(new ProductDto { Id = 3, Name = "phone n° 3" });
            phones.Add(new ProductDto { Id = 4, Name = "phone n° 4" });

            mockProductService
                .Setup(r => r.GetProducts()).Returns(Task.FromResult(phones.AsEnumerable()));

            var controller = new ProductController(
                mockLogger.Object,
                mockProductService.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var actionResult = result;
            Assert.NotNull(actionResult);
            Assert.IsAssignableFrom<ActionResult<List<ProductDto>>>(actionResult);

            Assert.NotNull(actionResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnNotFound_Async()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductService = new Mock<IProductService>();

            IList<ProductDto> phones = null;

            mockProductService
                .Setup(r => r.GetProducts()).Returns(Task.FromResult(phones.AsEnumerable()));

            var controller = new ProductController(
                mockLogger.Object,
                mockProductService.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            Assert.NotNull(result);

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateOne_WhenCalled_ReturnProductModelResult_Async()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductService = new Mock<IProductService>();

            ProductDto dellBeforeInsert = new ProductDto
            {
                Id = 0,
                Name = "Dell",
                Description = "Description for Dell Computer",
                Price = 600
            };

            // Arrange
            ProductDto dellAfterInsert = new ProductDto
            {
                Id = (new Random()).Next(1, 100),
                Name = "Dell",
                Description = "Description for Dell Computer",
                Price = 600
            };

            mockProductService
                .Setup(r => r.InsertProduct(dellBeforeInsert)).Returns(Task.FromResult(dellAfterInsert));

            var controller = new ProductController(
                mockLogger.Object,
                mockProductService.Object);

            // Act
            var result = await controller.Post(new ProductPoco(dellBeforeInsert));

            // Assert
            Assert.NotNull(result);

            Assert.NotEqual(0, ((result.Result as CreatedAtActionResult).Value as ProductDto).Id);
        }

        [Fact]
        public async Task UpdateOne_WhenCalled_ReturnNoContentResult_Async()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductService = new Mock<IProductService>();

            ProductDto nuraBeforeUpdate = new ProductDto
            {
                Id = 20,
                Name = "Nura",
                Description = "Default description",
                Price = 300
            };

            // Arrange
            ProductDto nuraAfterUpdate = new ProductDto
            {
                Id = 20,
                Name = "Nura",
                Description = "Description updated",
                Price = 280
            };

            mockProductService
                .Setup(r => r.GetProduct(nuraBeforeUpdate.Id)).Returns(Task.FromResult(nuraBeforeUpdate));

            mockProductService
                 .Setup(r => r.UpdateProduct(nuraBeforeUpdate)).Returns(Task.FromResult(nuraAfterUpdate));

            var controller = new ProductController(
                mockLogger.Object,
                mockProductService.Object);

            // Act
            var actionResult = await controller.Put(nuraBeforeUpdate);

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status204NoContent, (actionResult as StatusCodeResult).StatusCode);
        }

        [Fact]
        public async Task DeleteOne_WhenCalledWithId_ReturnProductModelResult_Async()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductService = new Mock<IProductService>();

            ProductDto dellToDelete = new ProductDto
            {
                Id = 50,
                Name = "Dell nura",
                Description = null,
                Price = default
            };

            var controller = new ProductController(
                mockLogger.Object,
                mockProductService.Object);

            mockProductService
                .Setup(r => r.GetProduct(dellToDelete.Id)).Returns(Task.FromResult(dellToDelete));
            
            mockProductService
                .Setup(r => r.DeleteProduct(dellToDelete.Id)).Verifiable();

            // Act
            var actionResult = await controller.Delete(50);

            // Assert
            Assert.NotNull(actionResult);

            Assert.Equal(StatusCodes.Status204NoContent, (actionResult as StatusCodeResult).StatusCode);
        }

        [Fact]
        public async Task DeleteOne_WhenCalledWithWrongId_ReturnNotFoundResult_Async()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductService = new Mock<IProductService>();

            var controller = new ProductController(
                mockLogger.Object,
                mockProductService.Object);

            // Act
            var actionResult = await controller.Delete(0);

            // Assert
            Assert.NotNull(actionResult);

            Assert.Equal(StatusCodes.Status204NoContent, (actionResult as StatusCodeResult).StatusCode);
        }

        [Fact]
        public async Task DeleteOne_ThrowException_Async()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductService = new Mock<IProductService>();

            mockProductService
                .Setup(r => r.GetProduct(1)).Throws(new Exception("..."));

            var controller = new ProductController(
                mockLogger.Object,
                mockProductService.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(StatusCodes.Status500InternalServerError, (result as ObjectResult).StatusCode);
        }
    }
}
