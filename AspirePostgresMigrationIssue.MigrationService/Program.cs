using AspirePostgresMigrationIssue.Data;
using AspirePostgresMigrationIssue.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<AppDbContext>("pgDb");
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
