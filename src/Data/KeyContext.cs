using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UrlApi.Data
{
  public class KeyContext : DbContext
  {
    private readonly IConfiguration _config;
    public DbSet<KeyModel> Keys { get; set; }

    public KeyContext(DbContextOptions options, IConfiguration config) : base(options)
    {
      _config = config;
      Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       optionsBuilder.UseSqlServer(_config.GetConnectionString("UrlMapApi"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
            builder.Entity<KeyModel>()
              .HasData(new
              {
                  Key = "abcdef",
                  Id = new Guid()
              });

    }

  }
}
