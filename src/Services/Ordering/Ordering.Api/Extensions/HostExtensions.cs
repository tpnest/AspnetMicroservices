using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Api.Extensions;

public static class HostExtensions
{
    public static WebApplication MigrateDatabase<TContext>(this WebApplication host,
        Action<TContext, IServiceProvider> seeder,
        int? retry = 0) where TContext : DbContext
    {
        int retryCount = retry!.Value;
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetRequiredService<TContext>();

        try
        {
            logger.LogInformation("数据库迁移：{DbContextName}", nameof(TContext));
            InvokeSeeder(seeder, context, services);
            logger.LogInformation("数据库迁移：{DbContextName}", nameof(TContext));

        }
        catch (SqlException ex)
        {
            logger.LogError("上下文:{ContextName}迁移失败,错误:{err}", typeof(TContext).Name, ex);
            if (retryCount <= 10)
            {
                retryCount++;
                Thread.Sleep(2000);
                MigrateDatabase<TContext>(host, seeder, retryCount);

            }
        }
        return host;
    }

    private static void InvokeSeeder<TContext>(
        Action<TContext, IServiceProvider> seeder, 
        TContext context, 
        IServiceProvider services) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}
