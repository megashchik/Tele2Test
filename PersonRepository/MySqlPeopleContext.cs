using Microsoft.EntityFrameworkCore;
using DTO;
using Microsoft.Extensions.Configuration;

namespace PersonRepository
{
    internal class MySqlPeopleContext : DbContext
    {
        public DbSet<Person> People { get; private set; }

        public MySqlPeopleContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 27)));
        }
    }
}