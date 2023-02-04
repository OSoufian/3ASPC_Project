using MySql.Data.MySqlClient;

using Microsoft.EntityFrameworkCore;

public class MySQLConnection : DbContext
{

    //   Connexion à la base de données qui marche :
    //	  public MySQLConnection()
    //   {
    //       string connectionString = null;
    //       MySqlConnection cnn;
    //       connectionString = "server=localhost;database=iBay;uid=root;pwd=\"\";";
    //       cnn = new MySqlConnection(connectionString);
    //       try {
    //           cnn.Open();
    //           Console.WriteLine("Connection Open ! ");
    //           cnn.Close();
    //       } catch (Exception ex) {
    //           Console.WriteLine("Can not open connection ! ");
    //       }
    //   }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=;database=iBay");
        }
    }
}
