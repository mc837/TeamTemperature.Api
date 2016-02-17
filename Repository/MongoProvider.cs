﻿using MongoDB.Driver;

namespace Repository
{
    public class MongoProvider : IMongoProvider
    {
        private readonly IMongoDatabase _database;
        private string _collectionName;
        public MongoProvider()
        {
            var mongoHost = "mongodb://localhost";
            _database = new MongoClient(mongoHost).GetDatabase("teamTemperature");

        }
        public IMongoProvider ForCollection(string collectionName)
        {
            _collectionName = collectionName;
            return this;
        }
        public bool Insert<T>(T model)
        {
            var database = _database.GetCollection<T>(_collectionName);

            try
            {
                database.InsertOneAsync(model).GetAwaiter().GetResult();
            }
            catch (MongoWriteException mwx)
            {
                //maybe add some logging?
                // mwx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                return false;
            }
            return true;
        }

        public bool Update<T>(T model)
        {
            var database = _database.GetCollection<T>(_collectionName);
            var t = model.GetType();
            var prop = t.GetProperty("Id").GetValue(model);
            
            var filter = Builders<T>.Filter.Eq("_id", prop);
            try
            {
                database.ReplaceOneAsync(filter, model).GetAwaiter().GetResult();
            }
            catch (MongoWriteException)
            {
                return false;
            }
            return true;
        }
    }
}