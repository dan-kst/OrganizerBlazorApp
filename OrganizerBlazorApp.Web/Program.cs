using Microsoft.EntityFrameworkCore;

using OrganizerBlazorApp.Application.Services;
using OrganizerBlazorApp.Domain.Interfaces;
using OrganizerBlazorApp.Infrastructure.Data;
using OrganizerBlazorApp.Infrastructure.Repositories;
using OrganizerBlazorApp.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register AppDbContext with SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Data Source=organizer.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString,
        b => b.MigrationsAssembly("OrganizerBlazorApp.Infrastructure")));
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<TaskService>();

var app = builder.Build();
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
