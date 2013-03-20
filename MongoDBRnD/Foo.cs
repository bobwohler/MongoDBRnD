using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBRnD
{
    class Foo
    {

        [BsonId]
        [BsonElement("_id")]
        public Guid Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }


    }
}
