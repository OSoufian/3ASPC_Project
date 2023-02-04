using System;
using MySql.Data.MySqlClient;

public class MySQLConnection {

	public MySQLConnection() {
        string connectionString = null;
        MySqlConnection cnn;
        connectionString = "server=localhost;database=iBay;uid=root;pwd=\"\";";
        cnn = new MySqlConnection(connectionString);
        try {
            cnn.Open();
            Console.WriteLine("Connection Open ! ");
            cnn.Close();
        } catch (Exception ex) {
            Console.WriteLine("Can not open connection ! ");
        }

    }
}
