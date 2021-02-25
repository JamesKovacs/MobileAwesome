using System.Linq;
using System.Threading.Tasks;
using MobileAwesomeApp.Infrastructure.Mongo;
using MobileAwesomeApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MobileAwesomeApp.Services
{
    public interface IFeastService
    {
        Task<Feast> CreateNewFeastAsync(User creator);
    }

    public class FeastService : IFeastService
    {
        private readonly MongoClientWrapper _client;
        private readonly CollectionNamespace _foodWordsCollectionNamespace = CollectionNamespace.FromFullName("sample_restaurants.food_words");
        private readonly CollectionNamespace _feastCollectionNamespace = CollectionNamespace.FromFullName("sample_restaurants.feasts");

        public FeastService(MongoClientWrapper client)
        {
            _client = client;
        }

        public async Task<Feast> CreateNewFeastAsync(User creator)
        {
            var sampleStage = BsonDocument.Parse("{ $sample: { size: 4 } }");
            var pipeline = new EmptyPipelineDefinition<BsonDocument>()
                                .AppendStage<BsonDocument, BsonDocument, BsonDocument>(sampleStage);
            var query = await _client.GetCollection<BsonDocument>(_foodWordsCollectionNamespace).AggregateAsync(pipeline).ConfigureAwait(false);
            var words = await query.ToListAsync();

            var feastKey = string.Join("-", words.Select(x => x["word"]));

            var feast = new Feast
            {
                FeastKey = feastKey,
                Creator = creator
            };

            await _client.GetCollection<Feast>(_feastCollectionNamespace).InsertOneAsync(feast);

            return feast;
        }
    }
}
