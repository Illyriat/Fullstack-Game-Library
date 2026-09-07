using Game_Library_Service.Common.Exceptions.CustomExceptions;
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
    public class PublishersController_CreatePublisher
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PublishersController _controller;

        public PublishersController_CreatePublisher()
        {
            _dbContext = InMemoryDatabaseHelper.GetContext();

            var services = new ServiceCollection();
            services.AddSingleton(_dbContext);
            services.AddSingleton<IMediator, Mediator>();
            services.AddScoped<ICommandHandler<CreatePublisher.Command, CreatePublisher.Result>, CreatePublisher.Handler>();
            services.AddLogging();

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            _controller = new PublishersController(mediator);
        }

        [Fact]
        public async Task CreatePublisher_ValidRequest_PersistsAndReturns201WithCreatedPublisher()
        {
            // Act
            var result = await _controller.CreatePublisher(new CreatePublisher.Command { Name = "Nintendo" }, TestContext.Current.CancellationToken);

            // Assert
            var created = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(201, created.StatusCode);

            var payload = Assert.IsType<CreatePublisher.Result>(created.Value);
            Assert.Equal("Nintendo", payload.Name);
            Assert.True(payload.Id > 0);

            var persisted = await _dbContext.Publishers.FindAsync([payload.Id], TestContext.Current.CancellationToken);
            Assert.NotNull(persisted);
            Assert.Equal("Nintendo", persisted!.Name);
        }

        [Fact]
        public async Task CreatePublisher_TrimsName_BeforePersisting()
        {
            // Act
            var result = await _controller.CreatePublisher(new CreatePublisher.Command { Name = "  Sega  " }, TestContext.Current.CancellationToken);

            // Assert
            var created = Assert.IsType<ObjectResult>(result.Result);
            var payload = Assert.IsType<CreatePublisher.Result>(created.Value);
            Assert.Equal("Sega", payload.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreatePublisher_MissingName_ThrowsBadRequestException(string name)
        {
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() =>
                _controller.CreatePublisher(new CreatePublisher.Command { Name = name }, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task CreatePublisher_NameExceedsMaxLength_ThrowsBadRequestException()
        {
            // Arrange
            var name = new string('a', CreatePublisher.MaxNameLength + 1);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() =>
                _controller.CreatePublisher(new CreatePublisher.Command { Name = name }, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task CreatePublisher_DuplicateName_ThrowsConflictException()
        {
            // Arrange
            await new PublisherBuilder().WithName("Nintendo").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() =>
                _controller.CreatePublisher(new CreatePublisher.Command { Name = "Nintendo" }, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task CreatePublisher_DuplicateName_IsCaseInsensitive()
        {
            // Arrange
            await new PublisherBuilder().WithName("Nintendo").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() =>
                _controller.CreatePublisher(new CreatePublisher.Command { Name = "nintendo" }, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task CreatePublisher_NameMatchesSoftDeletedPublisher_ThrowsConflictException()
        {
            // Arrange
            var deleted = await new PublisherBuilder().WithName("Nintendo").BuildAndAddAsync(_dbContext, TestContext.Current.CancellationToken);
            deleted.Deleted = true;
            await _dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            // Act & Assert
            // Publisher.Name has a DB-wide unique index that isn't scoped by Deleted, so a
            // soft-deleted publisher's name still counts as a duplicate.
            await Assert.ThrowsAsync<ConflictException>(() =>
                _controller.CreatePublisher(new CreatePublisher.Command { Name = "Nintendo" }, TestContext.Current.CancellationToken));
        }
    }
}
