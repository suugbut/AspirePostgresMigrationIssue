var builder = DistributedApplication.CreateBuilder(args);

var pgServer = builder.AddPostgres("pgServer")
    .WithPgWeb();
var pgDb = pgServer.AddDatabase("pgDb");


var migration = builder.AddProject<Projects.AspirePostgresMigrationIssue_MigrationService>("migration")
    .WithReference(pgDb)
    .WaitFor(pgDb);

builder.AddProject<Projects.AspirePostgresMigrationIssue_Api>("api")
    .WithReference(pgDb)
    .WaitForCompletion(migration);

builder.Build().Run();
