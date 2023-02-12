using MySql.Data.MySqlClient;

using Microsoft.EntityFrameworkCore;
using iBay.Models;

public class MySQLConnection : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Product> Product { get; set; }
    // public DbSet<Cart> Cart { get; set; }

    public MySQLConnection()
    {
    }

    public MySQLConnection(DbContextOptions<MySQLConnection> options)
            : base(options) {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=;database=iBay");
        }
    }
}