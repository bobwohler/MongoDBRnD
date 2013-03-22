using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace MongoDBRnD
{
    [TestClass]
    public class MongoDBTests
    {
        private MongoClient _mongoClient = null;
        private MongoDatabase _database = null;
        // These tests use a MongoDB server instance hosted in an Azure IaaS VM.
        private string _connectionString = "mongodb://mongodbsvr.cloudapp.net:27017";
        private string _dbName = "test";
        private string _collectionName = "foo";
        private Guid _bobFooId = Guid.Parse("1A886002-E3A9-4983-A7CA-354B8BD4DE53");
        private Guid _hollyFooId = Guid.Parse("5610F094-1ECA-4444-97C6-FEA231B8FC4F");

        [TestInitialize()]
        public void Initialize()
        {
            _mongoClient = new MongoClient(_connectionString);
            MongoServer server = _mongoClient.GetServer();
            _database = server.GetDatabase(_dbName);
            _database.GetCollection<Foo>(_collectionName).RemoveAll();
        }

        [TestMethod]
        public void Add_Foo()
        {

            #region Test setup
            MongoCollection<Foo> fooCollection = _database.GetCollection<Foo>(_collectionName);
            fooCollection.RemoveAll();
            #endregion

            BsonDocument fooDocEthan = new BsonDocument {
                { "_id", Guid.NewGuid().ToString() },
                { "name", "Ethan"}
            };
            BsonDocument fooDocAbby = new BsonDocument {
                { "_id", Guid.NewGuid().ToString() },
                { "name", "Abby" }
            };
            
            fooCollection.Insert(fooDocEthan);
            fooCollection.Insert(fooDocAbby);

            Assert.AreEqual(fooCollection.FindAll().Count(), 2);

            #region Test cleanup
            fooCollection.RemoveAll();
            #endregion

        }

        //[TestMethod]
        //public void Get_All_Foo()
        //{
        //    MongoCollection<Foo> fooCollection = _database.GetCollection<Foo>(_collectionName);
        //    Assert.AreEqual(fooCollection.FindAll().Count(),2);
        //}

        [TestMethod]
        public void Get_Foo_ByID()
        {
            #region Test setup
            MongoCollection<Foo> fooCollection = _database.GetCollection<Foo>(_collectionName);
            fooCollection.RemoveAll();
            BsonDocument fooDocEthan = new BsonDocument {
                { "_id", _bobFooId },
                { "name", "Bob"}
            };
            BsonDocument fooDocAbby = new BsonDocument {
                { "_id", _hollyFooId },
                { "name", "Holly" }
            };

            fooCollection.Insert(fooDocEthan);
            fooCollection.Insert(fooDocAbby);
            #endregion

            QueryDocument query = new QueryDocument("_id", _bobFooId);
            Foo foo = fooCollection.FindOne(query);
            Assert.AreEqual(foo.Name, "Bob");
            
            #region Test cleanup
            fooCollection.RemoveAll();
            #endregion

        }

        [TestMethod]
        public void Get_Foo_ByName()
        {
            #region Test setup
            MongoCollection<Foo> fooCollection = _database.GetCollection<Foo>(_collectionName);
            fooCollection.RemoveAll();
            BsonDocument fooDocBob = new BsonDocument {
                { "_id", _bobFooId },
                { "name", "Bob"}
            };
            BsonDocument fooDocHolly = new BsonDocument {
                { "_id", _hollyFooId },
                { "name", "Holly" }
            };

            fooCollection.Insert(fooDocBob);
            fooCollection.Insert(fooDocHolly);
            #endregion
            
            QueryDocument query = new QueryDocument("name", "Holly");
            Foo foo = fooCollection.FindOne(query);
            Assert.AreEqual(foo.Id, _hollyFooId);

            #region Test cleanup
            fooCollection.RemoveAll();
            #endregion

        }

    }
}
