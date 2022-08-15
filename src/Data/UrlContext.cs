using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UrlApi.Data
{
  public class UrlContext : DbContext
  {
    private readonly IConfiguration _config;

    public UrlContext(DbContextOptions<UrlContext> options, IConfiguration config) : base(options)
    {
      _config = config;
    }

    public DbSet<UrlModel> Urls { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(_config.GetConnectionString("UrlMapApi"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<UrlDbModel>()
        .HasData(new 
        {
            Key = "abcdef",
            Protocol = "https://",
            Host = "www.youtube.com/",
            Path = "watch?v=Qg6BDgS9Dfw"
        });

    }

  }
}
