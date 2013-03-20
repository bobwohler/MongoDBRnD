using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;

namespace MongoDBRnD
{
    [TestClass]
    public class MongoDBTests
    {
        private MongoServer _mongoServer = null;
        private MongoClient _mongoClient = null;
        private MongoDatabase _database = null;
        private bool _disposed = false;
        private string _connectionString = "mongodb://mongodbsvr.cloudapp.net:27017";
        private string _dbName = "test";
        private string _collectionName = "foo";
        private Guid _fooId = Guid.Parse("1A886002-E3A9-4983-A7CA-354B8BD4DE53");

        [TestInitialize()]
        public void Initialize()
        {
            _mongoClient = new MongoClient(_connectionString);
            MongoServer server = _mongoClient.GetServer();
            _database = server.GetDatabase(_dbName);
        }

        [TestMethod]
        public void Get_All_Foo()
        {
            MongoCollection<Foo> fooCollection = _database.GetCollection<Foo>(_collectionName);
            Assert.AreEqual(fooCollection.FindAll().Count(),2);
        }

        [TestMethod]
        public void Get_Foo_ByID()
        {
            MongoCollection<Foo> fooCollection = _database.GetCollection<Foo>(_collectionName);
            QueryDocument query = new QueryDocument("_id", "1A886002-E3A9-4983-A7CA-354B8BD4DE53");
            Foo foo = fooCollection.FindOne(query);
            Assert.AreEqual(foo.Name, "Bob");
        }

        [TestMethod]
        public void Get_Foo_ByName()
        {
            MongoCollection<Foo> fooCollection = _database.GetCollection<Foo>(_collectionName);
            QueryDocument query = new QueryDocument("name", "Holly");
            Foo foo = fooCollection.FindOne(query);
            Assert.AreEqual(foo.Id, Guid.Parse("5610F094-1ECA-4444-97C6-FEA231B8FC4F"));
        }
    }
}
