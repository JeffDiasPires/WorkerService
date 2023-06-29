using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using WorkerService;

    public class Client
    {
        [JsonIgnore]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [Required]
        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [Required]
        [BsonElement("LastName")]
        public string LastName { get; set; }

        [Required]
        [BsonElement("Birthday")]
        public DateTime? Birthday { get; set; }
          

        [Required]
        [BsonElement("Age")]
        public int? Age { get; set; }

        [Required]
        [BsonElement("Phone")]
        
        public string Phone { get; set; }

        [Required]
        [BsonElement("Document")]
        public string Document { get; set; }

 
    }

