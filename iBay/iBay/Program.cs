
using MySql.Data.MySqlClient;

namespace iBay {
    public class Program {
        public static void Main(string[] args) {
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

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}