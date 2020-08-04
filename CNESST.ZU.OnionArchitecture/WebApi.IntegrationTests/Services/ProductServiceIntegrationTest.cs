using Application;
using Application.DTOs;
using Application.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence;
using Persistence.Context;
using System;
using System.Threading.Tasks;
using Xunit;

namespace WebApi.IntegrationTests.Services
{
    public class ProductServiceIntegrationTest
    {
        /// <summary>
        /// Create DbContextOptions
        /// </summary>
        /// <returns>new DbContextOptions</returns>
        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            builder
                .UseInMemoryDatabase(databaseName: "CNESST_Db_InMemory")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Fact]
        public async Task AddProduct_CalledWithValidObject_ReturnNewProduct_Async()
        {
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                // Insert seed data into the database using one instance of the context
                context.Populate();

                // Arrange
                #region Logger
                var mockLoggerUOW = new Mock<ILogger<UnitOfWork>>();
                #endregion
                #region Mapper
                var mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(typeof(ApplicationProfile));
                });

                var mapper = mapperConfiguration.CreateMapper();
                #endregion
                #region UOW
                var uow = new UnitOfWork(context, mockLoggerUOW.Object);
                #endregion


                var product = new ProductDto
                {
                    Name = "cK One",
                    Description = "My bestof parfum by Calvin Klein",
                    Price = 25
                };

                // Creation du service
                var srv = new ProductService(mapper, uow);

                // Affect
                var productAdded = await srv.InsertProduct(product);

                // Assert
                Assert.NotNull(productAdded);
                Assert.NotEqual(0, productAdded.Id);
            }
        }

        [Fact]
        public async Task DeleteProduct_CalledWithValidId_ReturnNoException_Async()
        {
            try
            {
                using (var context = new ApplicationDbContext(CreateNewContextOptions()))
                {
                    // Insert seed data into the database using one instance of the context
                    context.Populate();

                    // Arrange
                    #region Logger
                    var mockLoggerUOW = new Mock<ILogger<UnitOfWork>>();
                    #endregion
                    #region Mapper
                    var mapperConfiguration = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile(typeof(ApplicationProfile));
                    });

                    var mapper = mapperConfiguration.CreateMapper();
                    #endregion
                    #region UOW
                    var uow = new UnitOfWork(context, mockLoggerUOW.Object);
                    #endregion

                    var productId = 2;

                    // Creation du service
                    var srv = new ProductService(mapper, uow);

                    // Affect
                    await srv.DeleteProduct(productId);
                }

                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }
    }
}
