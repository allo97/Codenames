using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TajniacyAPI.DataAccess.Model.Interfaces;

namespace TajniacyAPI.DataAccess.Model
{
    public class SelectionCard : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public CardType CardType { get; set; }
    }
}
