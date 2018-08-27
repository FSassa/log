using System;
using System.Linq;

using log4net.Core;
using log4net.Appender;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Conventions;

namespace SSTI.Log.Log4Net.MongoDB.Appenders
{
    public class MongoDBTypeAppender : AppenderSkeleton
    {
        public string CollectionName    { get; set; }
        public string ConnectionString  { get; set; }
        public Type   DocumentType      { get; set; }
        public bool   Capped            { get; set; }
        public long   MaxDocuments      { get; set; }
        public long   MaxSize           { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var collection = GetCollection();

            collection.InsertOneAsync(BuildBsonDocument(loggingEvent));
        }

        protected virtual IMongoCollection<BsonDocument> GetCollection()
        {
            var mongoDb = GetDatabase();

            var exists  = mongoDb.ListCollectionsAsync(new ListCollectionsOptions { Filter = new BsonDocument("name", CollectionName) })
                                 .Result
                                 .ToListAsync()
                                 .Result
                                 .Any();

            if (!exists)
            {
                mongoDb.CreateCollectionAsync(CollectionName, new CreateCollectionOptions { Capped = Capped, MaxDocuments = MaxDocuments, MaxSize = MaxSize }).ConfigureAwait(true);
            }

            return mongoDb.GetCollection<BsonDocument>(CollectionName);
        }

        public IMongoDatabase GetDatabase()
        {
            var mongoDbUrl = MongoUrl.Create(ConnectionString);
            var mongoDbCli = new MongoClient(mongoDbUrl);

            ConventionRegistry.Register("CaseConvention", new ConventionPack { new CamelCaseElementNameConvention() }, type => true);

            return mongoDbCli.GetDatabase(mongoDbUrl.DatabaseName);
        }

        protected virtual BsonDocument BuildBsonDocument(LoggingEvent log)
        {
            var document = log.MessageObject.ToBsonDocument(DocumentType);

            return document;
        }
    }
}