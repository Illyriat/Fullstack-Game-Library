using Game_Library_Service.Common.Mediator;
using Game_Library_Service.Common.Mediator.Interfaces;
using Game_Library_Service.Data.Contexts;
using Game_Library_Service.Features.Game_Lib_FE.Controllers;
using Game_Library_Service.Features.Game_Lib_FE.Logic;
using Game_Library_Service.Tests.Data.Builders;
using Game_Library_Service.Tests.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Game_Library_Service.Tests.Features.Game_Lib_FE.Controllers.GamesControllerTests
{
    public class GamesController_GetGames
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly GamesController _controller;

        public GamesController_GetGames()
        {
            _dbContext = InMemoryDatabaseHelper.GetContext();

            var services = new ServiceCollection();
            services.AddSingleton(_dbContext);
            services.AddSingleton<IMediator, Mediator>();
            services.AddScoped<IQueryHandler<GetGames.Query, GetGames.Result>, GetGames.Handler>();
            services.AddLogging();

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            _controller = new GamesController(mediator);
        }

        [Fact]
        public async Task GetGames_NoFilters_ReturnsAllGamesOrderedByName()
        {
            // Arrange
            await new GameBuilder().WithName("Zelda").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            await new GameBuilder().WithName("Elden Ring").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act
            var result = await _controller.GetGames(null, null, null, null, 1, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetGames.Result>(ok.Value);

            Assert.Equal(2, payload.TotalCount);
            Assert.Equal("Elden Ring", payload.Items[0].Name);
            Assert.Equal("Zelda", payload.Items[1].Name);
        }

        [Fact]
        public async Task GetGames_FilterByName_IsCaseInsensitiveContains()
        {
            // Arrange
            await new GameBuilder().WithName("Elden Ring").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            await new GameBuilder().WithName("Mario Kart").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act - search lowercase, mid-word
            var result = await _controller.GetGames("elden", null, null, null, 1, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetGames.Result>(ok.Value);

            Assert.Single(payload.Items);
            Assert.Equal("Elden Ring", payload.Items[0].Name);
        }

        [Fact]
        public async Task GetGames_FilterByReleaseYear_ReturnsOnlyMatchingYear()
        {
            // Arrange
            await new GameBuilder().WithName("Game A").WithReleaseYear(2017).BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            await new GameBuilder().WithName("Game B").WithReleaseYear(2017).BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            await new GameBuilder().WithName("Game C").WithReleaseYear(2022).BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act
            var result = await _controller.GetGames(null, 2017, null, null, 1, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetGames.Result>(ok.Value);

            Assert.Equal(2, payload.TotalCount);
            Assert.All(payload.Items, g => Assert.Equal(2017, g.ReleaseYear));
        }

        [Fact]
        public async Task GetGames_FilterByGenre_ReturnsOnlyMatchingGenre()
        {
            // Arrange
            var racing = await new GenreBuilder().WithName("Racing").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            var fighting = await new GenreBuilder().WithName("Fighting").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            await new GameBuilder().WithName("Racer").WithGenre(racing).BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            await new GameBuilder().WithName("Fighter").WithGenre(fighting).BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act
            var result = await _controller.GetGames(null, null, racing.Id, null, 1, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetGames.Result>(ok.Value);

            Assert.Single(payload.Items);
            Assert.Equal("Racer", payload.Items[0].Name);
        }

        [Fact]
        public async Task GetGames_FilterByPublisherId_ReturnsOnlyGamesFromThatPublisher()
        {
            // Arrange
            var nintendo = await new PublisherBuilder().WithName("Nintendo").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            var fromSoftware = await new PublisherBuilder().WithName("FromSoftware").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            await new GameBuilder().WithName("Zelda").WithPublisher(nintendo).BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            await new GameBuilder().WithName("Elden Ring").WithPublisher(fromSoftware).BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act
            var result = await _controller.GetGames(null, null, null, nintendo.Id, 1, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetGames.Result>(ok.Value);

            Assert.Single(payload.Items);
            Assert.Equal("Zelda", payload.Items[0].Name);
            Assert.Equal("Nintendo", payload.Items[0].PublisherName);
        }

        [Fact]
        public async Task GetGames_CombinedFilters_AppliesAllFiltersTogether()
        {
            // Arrange
            var nintendo = await new PublisherBuilder().WithName("Nintendo").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            var adventure = await new GenreBuilder().WithName("Adventure").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            var racing = await new GenreBuilder().WithName("Racing").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            await new GameBuilder()
                .WithName("Zelda: Breath of the Wild")
                .WithReleaseYear(2017)
                .WithGenre(adventure)
                .WithPublisher(nintendo)
                .BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            await new GameBuilder()
                .WithName("Mario Kart 8")
                .WithReleaseYear(2017)
                .WithGenre(racing)
                .WithPublisher(nintendo)
                .BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act
            var result = await _controller.GetGames("zelda", 2017, adventure.Id, nintendo.Id, 1, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetGames.Result>(ok.Value);

            Assert.Single(payload.Items);
            Assert.Equal("Zelda: Breath of the Wild", payload.Items[0].Name);
        }

        [Fact]
        public async Task GetGames_ExcludesSoftDeletedGames()
        {
            // Arrange
            var active = await new GameBuilder().WithName("Active Game").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            var deleted = await new GameBuilder().WithName("Deleted Game").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            deleted.Deleted = true;
            await _dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            // Act
            var result = await _controller.GetGames(null, null, null, null, 1, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetGames.Result>(ok.Value);

            Assert.Single(payload.Items);
            Assert.Equal(active.Id, payload.Items[0].Id);
        }

        [Fact]
        public async Task GetGames_Pagination_ReturnsFirstFiftyAndReportsCorrectTotals()
        {
            // Arrange
            for (var i = 1; i <= 75; i++)
            {
                await new GameBuilder().WithName($"Game {i:D3}").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            }

            // Act
            var page1Result = await _controller.GetGames(null, null, null, null, 1, TestContext.Current.CancellationToken);
            var page2Result = await _controller.GetGames(null, null, null, null, 2, TestContext.Current.CancellationToken);

            // Assert
            var page1Ok = Assert.IsType<OkObjectResult>(page1Result.Result);
            var page1 = Assert.IsType<GetGames.Result>(page1Ok.Value);

            Assert.Equal(50, page1.Items.Count);
            Assert.Equal(75, page1.TotalCount);
            Assert.Equal(2, page1.TotalPages);
            Assert.Equal(1, page1.Page);
            Assert.Equal(50, page1.PageSize);

            var page2Ok = Assert.IsType<OkObjectResult>(page2Result.Result);
            var page2 = Assert.IsType<GetGames.Result>(page2Ok.Value);

            Assert.Equal(25, page2.Items.Count);
        }

        [Fact]
        public async Task GetGames_PageBelowOne_TreatedAsPageOne()
        {
            // Arrange
            await new GameBuilder().WithName("Only Game").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act
            var result = await _controller.GetGames(null, null, null, null, 0, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetGames.Result>(ok.Value);

            Assert.Equal(1, payload.Page);
            Assert.Single(payload.Items);
        }

        [Fact]
        public async Task GetGames_NoResultsMatchFilter_ReturnsEmptyItemsWithZeroTotals()
        {
            // Arrange
            var action = await new GenreBuilder().WithName("Action").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            var horror = await new GenreBuilder().WithName("Horror").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            await new GameBuilder().WithName("Some Game").WithGenre(action).BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act
            var result = await _controller.GetGames(null, null, horror.Id, null, 1, TestContext.Current.CancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<GetGames.Result>(ok.Value);

            Assert.Empty(payload.Items);
            Assert.Equal(0, payload.TotalCount);
            Assert.Equal(0, payload.TotalPages);
        }
    }
}
