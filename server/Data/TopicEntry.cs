using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations;
using server.Models;

namespace server.Data
{
    public class TopicEntry
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        public string Description { get; set; }
        
        public int HotScore { get; set; }

        public ChoronicleRecord ChoronicleRecord { get; set; }
        public int ChoronicleRecordId { get; set; }
    }
}