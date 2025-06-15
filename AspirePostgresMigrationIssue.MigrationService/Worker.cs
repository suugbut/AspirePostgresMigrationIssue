using AspirePostgresMigrationIssue.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AspirePostgresMigrationIssue.MigrationService;

public class Worker(
    ILogger<Worker> logger,
    IServiceProvider provider,
    IHostApplicationLifetime lifetime
    ) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private readonly ActivitySource _activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cantok)
    {
        using var activity = _activitySource.StartActivity("Migrating database.", ActivityKind.Client);
        try
        {
            var scope = provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await MigrateDatabaseAsync(db, cantok);
            await SeedDatabaseAsync(db, cantok);
            logger.LogInformation("Database migration completed successfully.");
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }
        lifetime.StopApplication();
    }

    private async Task MigrateDatabaseAsync(AppDbContext db, CancellationToken cantok)
    {
        var strategy = db.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(db.Database.MigrateAsync, cantok);
    }
    private async Task SeedDatabaseAsync(AppDbContext db, CancellationToken cantok)
    {
        var students = new[]
        {
            new Student { FullName = "Alice Joker" },
            new Student { FullName = "Bob Taylor" },
            new Student { FullName = "Charlie Blacksmith" }
        };

        var strategy = db.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await db.Database.BeginTransactionAsync(cantok);
            await db.Students.AddRangeAsync(students, cantok);
            await db.SaveChangesAsync(cantok);
            await transaction.CommitAsync(cantok);
        });
    }
}
