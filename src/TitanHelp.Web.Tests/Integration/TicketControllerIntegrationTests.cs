using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http.Json;
using TitanHelp.DataAccess.Data;
using TitanHelp.DataAccess.Entities;

namespace TitanHelp.Web.Tests.Integration
{
    [TestClass]
    public class TicketsControllerIntegrationTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [TestInitialize]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove existing DbContext
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        // Add in-memory database
                        services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("IntegrationTestDb_" + Guid.NewGuid());
                        });

                        // Build service provider and seed database
                        var sp = services.BuildServiceProvider();
                        using (var scope = sp.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            db.Database.EnsureCreated();
                        }
                    });
                });

            _client = _factory.CreateClient();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        [TestMethod]
        public async Task Get_Index_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/Ticket/Index");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_Index_ReturnsHtmlContentType()
        {
            // Act
            var response = await _client.GetAsync("/Ticket/Index");

            // Assert
            Assert.AreEqual("text/html; charset=utf-8",
                response.Content.Headers.ContentType?.ToString());
        }

        [TestMethod]
        public async Task Get_Index_ContainsExpectedContent()
        {
            // Act
            var response = await _client.GetAsync("/Ticket/Index");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.IsTrue(content.Contains("TitanHelp"));
            Assert.IsTrue(content.Contains("Tickets") || content.Contains("tickets"));
        }

        [TestMethod]
        public async Task Get_Create_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/Ticket/Create");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_Create_ContainsForm()
        {
            // Act
            var response = await _client.GetAsync("/Ticket/Create");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.IsTrue(content.Contains("<form"));
            Assert.IsTrue(content.Contains("Name") || content.Contains("name"));
            Assert.IsTrue(content.Contains("Description") || content.Contains("description"));
        }

        [TestMethod]
        public async Task Get_Details_WithValidId_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/Ticket/Details/1");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        public async Task Get_Details_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/Ticket/Details/9999");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_Edit_WithValidId_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/Ticket/Edit/1");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        public async Task Get_Edit_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/Ticket/Edit/9999");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_Delete_WithValidId_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/Ticket/Delete/1");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        public async Task Get_Delete_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/Ticket/Delete/9999");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Application_LoadsSuccessfully()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert - Should redirect to Ticket/Index
            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
        }

        [TestMethod]
        public async Task StaticFiles_AreAccessible()
        {
            // Act - Try to access bootstrap CSS
            var response = await _client.GetAsync("/lib/bootstrap/dist/css/bootstrap.min.css");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}