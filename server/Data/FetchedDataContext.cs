using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    public class FetchedDataContext : DbContext
    {
        public FetchedDataContext(DbContextOptions<FetchedDataContext> options)
            : base(options) { }
        
        public DbSet<ChoronicleRecord> ChoronicleRecords { get; set; }

        public DbSet<TopicEntry> TopicEntries { get; set; }
    }
}