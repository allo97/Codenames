﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TajniacyAPI.MongoAPI.Interfaces;

namespace TajniacyAPI.CardsManagement.DataAccess.Model
{
    public class WordCard : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public string Word { get; set; }
    }
}
