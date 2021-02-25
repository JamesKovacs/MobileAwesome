using System.Collections.Generic;
using MongoDB.Bson;

namespace MobileAwesomeApp.Models
{
    public class Feast
    {
        public ObjectId Id { get; private set; }
        public string FeastKey { get; set; }
        public User Creator { get; set; }
        public IEnumerable<User> Participants { get; private set; }
    }
}
