using System;
using ApiDotNet.Data.Collections;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace ApiDotNet.Data
{
    public class MongoDB
    {
        public IMongoDatabase mongoDatabase { get; }

        public MongoDB(IConfiguration configuration)
        {
            try
            {
                var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["ConnectionStrings"]));
                var client = new MongoClient(settings);
                mongoDatabase = client.GetDatabase(configuration["BankName"]);
                MapClasses();
            }
            catch (Exception exception)
            {
                throw new MongoException("It was ot possible to connect to MongoDB", exception);
            }
        }
        public void MapClasses()
        {
            var conventionPack = new ConventionPack{new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            if (!BsonClassMap.IsClassMapRegistered(typeof(Infected)))
            {
                BsonClassMap.RegisterClassMap<Infected>(i =>
                {
                    i.AutoMap();
                    i.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}