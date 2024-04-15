using Microsoft.EntityFrameworkCore;
using ZooDemoAPI.Model;

namespace ZooDemoAPI.Data
{
    public class ZooDbContext : DbContext
    {
        public DbSet<Zoo> Zoos { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<ZooAnimal> ZooAnimals { get; set; }

        public ZooDbContext(DbContextOptions<ZooDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your model here
            modelBuilder.Entity<Zoo>().ToTable("Zoo");
            modelBuilder.Entity<Animal>().ToTable("Animal");
            modelBuilder.Entity<ZooAnimal>().ToTable("ZooAnimal");
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // Configure the database connection
        //    optionsBuilder.UseSqlServer("Data Source=MS-IN-DES-04;Initial Catalog=WPFZOO;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True");
        //}
    }
}
