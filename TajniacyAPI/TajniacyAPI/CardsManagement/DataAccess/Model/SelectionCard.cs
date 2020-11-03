using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TajniacyAPI.CardsManagement.DataAccess.Model.Interfaces;

namespace TajniacyAPI.CardsManagement.DataAccess.Model
{
    public class SelectionCard : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public CardType CardType { get; set; }
    }
}
