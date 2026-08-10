using Game_Library_Service.Data.Contexts;
using Game_Library_Service.Data.Entities;

namespace Game_Library_Service.Tests.Data.Builders
{
    public class PublisherBuilder
    {
        private string _name = "Default Publisher";

        public PublisherBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Publisher Build()
        {
            return new Publisher
            {
                Name = _name
            };
        }

        public async Task<Publisher> BuildAndAddAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            var publisher = Build();
            context.Publishers.Add(publisher);
            await context.SaveChangesAsync(cancellationToken);
            return publisher;
        }
    }
}
