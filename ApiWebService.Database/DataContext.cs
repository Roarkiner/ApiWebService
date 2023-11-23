namespace ApiWebService;

using ApiWebService.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(Configuration.GetConnectionString("Database"),
            o => o.MigrationsAssembly("ApiWebService.Api"));
    }

    public DbSet<Note> Notes { get; set; }
    public DbSet<Person> Persons { get; set; }
}