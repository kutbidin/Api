using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlApi.Data
{
    public class AppDataContext : DbContext
    {
        public DbSet<UrlDbModel> Urls { get; set; }
        public DbSet<KeyModel> Keys { get; set; }
        private string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ApiDb";
        public AppDataContext()
        {
            Database.EnsureCreated();
            KeyModel[] keyModels = new KeyModel[100];
            for (int i = 1; i <= 100; i++)
            {
                KeyModel keyModel = new KeyModel();
                string key = Convert.ToBase64String((Guid.NewGuid()).ToByteArray()).Replace("/","R").Replace("=", "").Replace("+", "").Substring(0, 6);
                keyModel.Key = key;
                keyModels[i - 1] = keyModel;
            }
            Keys.AddRange(keyModels);
            this.SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //KeyModel[] keyModels=new KeyModel[1000];
            //for(int i = 1; i <= 1000; i++)
            //{
            //    KeyModel keyModel = new KeyModel();
            //    string key = Convert.ToBase64String((Guid.NewGuid()).ToByteArray()).Replace("=","").Replace("+","").Substring(0,6);
            //    keyModel.Key = key;
            //    keyModel.Id = i;
            //    keyModels[i-1]=keyModel;
            //}
            //modelBuilder.Entity<KeyModel>().HasData(keyModels);
            modelBuilder.Entity<KeyModel>()
               .Property(a => a.Id).UseSqlServerIdentityColumn();  
            modelBuilder.Entity<UrlDbModel>()
               .Property(a => a.Id).UseSqlServerIdentityColumn();
                  //.HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            base.OnModelCreating(modelBuilder);
        }
    }
}
