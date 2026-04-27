using Microsoft.EntityFrameworkCore;

using OrganizerBlazorApp.Domain.Interfaces;
using OrganizerBlazorApp.Infrastructure.Data;
using OrganizerBlazorApp.Infrastructure.Repositories;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<WebApplication>()
app.MapRazorComponents<WebApplication>()
    .AddInteractiveServerRenderMode();

app.Run();
