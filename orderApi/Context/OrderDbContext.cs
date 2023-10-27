using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Npgsql;
using orderApi.Models;

namespace basketApi.Context
{
    public class OrderDbContext: Microsoft.EntityFrameworkCore.DbContext
    {
		public OrderDbContext():base(){ }
		public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }	
		public DbSet<Order> Orders { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string db_setting = String.Format("Host={0}; Port={1}; Username={2}; Password={3}; Database={4};", "localhost", 5432, "root", "root", "Emarket");

            string db_setting = String.Format("Host={0}; Port={1}; Username={2}; Password={3}; Database={4};", "192.168.0.14", 5432, "root", "root", "Emarket");

            optionsBuilder.UseNpgsql(db_setting);
        }
    }
}
