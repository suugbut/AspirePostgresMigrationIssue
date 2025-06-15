using AspirePostgresMigrationIssue.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<AppDbContext>("pgDb");
var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/",  (AppDbContext db, CancellationToken cantok) => db.Students.ToArrayAsync(cantok));

app.Run();
