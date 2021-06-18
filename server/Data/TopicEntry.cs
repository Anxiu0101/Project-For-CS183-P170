using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations;

namespace server.Data
{
    public class TopicEntry
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        public string Description { get; set; }
        
        public int HotScore { get; set; }

        public FetchedDataContext FetchedDataContext { get; set; }
        public int FetchedDataContextId { get; set; }
    }
}