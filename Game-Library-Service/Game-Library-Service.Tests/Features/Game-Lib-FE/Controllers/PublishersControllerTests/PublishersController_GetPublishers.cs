using Game_Library_Service.Common.Mediator;
using Game_Library_Service.Common.Mediator.Interfaces;
using Game_Library_Service.Data.Contexts;
using Game_Library_Service.Features.Game_Lib_FE.Controllers;
using Game_Library_Service.Features.Game_Lib_FE.Logic;
using Game_Library_Service.Tests.Data.Builders;
using Game_Library_Service.Tests.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Game_Library_Service.Tests.Features.Game_Lib_FE.Controllers.PublishersControllerTests
{
    public class PublishersController_GetPublishers
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PublishersController _controller;

        public PublishersController_GetPublishers()
        {
            _dbContext = InMemoryDatabaseHelper.GetContext();

            var services = new ServiceCollection();
            services.AddSingleton(_dbContext);
            services.AddSingleton<IMediator, Mediator>();
            services.AddScoped<IQueryHandler<GetPublishers.Query, GetPublishers.Result>, GetPublishers.Handler>();
            services.AddLogging();

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            _controller = new PublishersController(mediator);
        }

        [Fact]
        public async Task GetPublishers_NoFilters_ReturnsAllPublishersOrderedByName()
        {
            // Arrange
            await new PublisherBuilder().WithName("Nintendo").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            await new PublisherBuilder().WithName("FromSoftware").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act
            var result = await _controller.GetPublishers(null, 1, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetPublishers.Result>(ok.Value);

            Assert.Equal(2, payload.TotalCount);
            Assert.Equal("FromSoftware", payload.Items[0].Name);
            Assert.Equal("Nintendo", payload.Items[1].Name);
        }

        [Fact]
        public async Task GetPublishers_FilterByName_IsCaseInsensitiveContains()
        {
            // Arrange
            await new PublisherBuilder().WithName("Nintendo").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            await new PublisherBuilder().WithName("FromSoftware").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act - search lowercase, mid-word
            var result = await _controller.GetPublishers("nint", 1, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetPublishers.Result>(ok.Value);

            Assert.Single(payload.Items);
            Assert.Equal("Nintendo", payload.Items[0].Name);
        }

        [Fact]
        public async Task GetPublishers_ExcludesSoftDeletedPublishers()
        {
            // Arrange
            var active = await new PublisherBuilder().WithName("Active Publisher").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            var deleted = await new PublisherBuilder().WithName("Deleted Publisher").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            deleted.Deleted = true;
            await _dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            // Act
            var result = await _controller.GetPublishers(null, 1, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetPublishers.Result>(ok.Value);

            Assert.Single(payload.Items);
            Assert.Equal(active.Id, payload.Items[0].Id);
        }

        [Fact]
        public async Task GetPublishers_Pagination_ReturnsFirstFiftyAndReportsCorrectTotals()
        {
            // Arrange
            for (var i = 1; i <= 75; i++)
            {
                await new PublisherBuilder().WithName($"Publisher {i:D3}").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            }

            // Act
            var page1Result = await _controller.GetPublishers(null, 1, TestContext.Current.CancellationToken);
            var page2Result = await _controller.GetPublishers(null, 2, TestContext.Current.CancellationToken);

            // Assert
            var page1Ok = Assert.IsType<OkObjectResult>(page1Result.Result);
            var page1 = Assert.IsType<GetPublishers.Result>(page1Ok.Value);

            Assert.Equal(50, page1.Items.Count);
            Assert.Equal(75, page1.TotalCount);
            Assert.Equal(2, page1.TotalPages);
            Assert.Equal(1, page1.Page);
            Assert.Equal(50, page1.PageSize);

            var page2Ok = Assert.IsType<OkObjectResult>(page2Result.Result);
            var page2 = Assert.IsType<GetPublishers.Result>(page2Ok.Value);

            Assert.Equal(25, page2.Items.Count);
        }

        [Fact]
        public async Task GetPublishers_PageBelowOne_TreatedAsPageOne()
        {
            // Arrange
            await new PublisherBuilder().WithName("Only Publisher").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act
            var result = await _controller.GetPublishers(null, 0, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetPublishers.Result>(ok.Value);

            Assert.Equal(1, payload.Page);
            Assert.Single(payload.Items);
        }

        [Fact]
        public async Task GetPublishers_NoResultsMatchFilter_ReturnsEmptyItemsWithZeroTotals()
        {
            // Arrange
            await new PublisherBuilder().WithName("Nintendo").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act
            var result = await _controller.GetPublishers("Sega", 1, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetPublishers.Result>(ok.Value);

            Assert.Empty(payload.Items);
            Assert.Equal(0, payload.TotalCount);
            Assert.Equal(0, payload.TotalPages);
        }
    }
}
