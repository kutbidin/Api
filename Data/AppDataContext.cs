using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using UrlMaker.Models;

namespace UrlMaker.Data
{
    public class AppDataContext:DbContext
    {
        //db entities
        private readonly IConfiguration _config;
        public DbSet<UrlModel> Urls { get; set; }
        public DbSet<KeyModel> Keys { get; set; }
        //private readonly string connString = "Server=atgdevargedb01;Database=API_MANAGEMENT;Trusted_Connection=True;";
        public AppDataContext(IConfiguration config) 
        {
            _config = config;
            Database.EnsureCreated();
            KeyModel[] keys = new KeyModel[10];
            for (int i = 0; i < 10; i++)
            {
                keys[i] = new KeyModel();
                keys[i].Key = Guid.NewGuid().ToString().Substring(0, 6);
            }
            Keys.AddRange(keys);
            this.SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("ApiDb"));
            //base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<KeyModel>().HasNoKey();
            modelBuilder.Entity<KeyModel>().Property(x => x.Id).UseIdentityColumn(1, 1);
            modelBuilder.Entity<UrlModel>().Property(x => x.Id).UseIdentityColumn(1, 1);
            base.OnModelCreating(modelBuilder);
        }

    }
}
