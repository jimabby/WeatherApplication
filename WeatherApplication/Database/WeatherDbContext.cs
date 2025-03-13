using WeatherApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace WeatherApplication.Database
{
    class WeatherDbContext: DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
        : base(options) { }
        public DbSet<WeatherRecord> WeatherRecords { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherRecord>().ToTable("WeatherRecords");
        }
    }
}
