using Npgsql;

namespace Discount.Grpc.Extensions;

public static class MigrateExtensions
{
    public static WebApplication MigrateDatabase<TContext>(this WebApplication app, int? retry = 0)
    {
        var config = app.Services.GetRequiredService<IConfiguration>();
        var logger = app.Services.GetRequiredService<ILogger<TContext>>();
        var retryFor = retry!.Value;

        try
        {
            logger.LogInformation("Migrating Postgresql Database...");
            using var conn = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
            conn.Open();
            using var command = new NpgsqlCommand();
            command.Connection = conn;
            
            command.CommandText = "DROP TABLE IF EXISTS Coupon";
            command.ExecuteNonQuery();

            command.CommandText =
                @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, ProductName VARCHAR(24) NOT NULL, Description TEXT, Amount INT)";
            command.ExecuteNonQuery();

            command.CommandText =
                @"INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
            command.ExecuteNonQuery();
            
            command.CommandText =
                @"INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
            command.ExecuteNonQuery();
            
            logger.LogInformation(("Migration completed."));
        }
        catch (Exception e)
        {
            if (retryFor < 10)
            {
                logger.LogError(e, "An error occured while migrating the database");
                Task.Delay(2000);
                retryFor++;
                MigrateDatabase<TContext>(app, retryFor);
            }
        }

        return app;
    }
}