using Microsoft.EntityFrameworkCore;
using TitanHelp.DataAccess.Data;
using TitanHelp.DataAccess.Interfaces;
using TitanHelp.DataAccess.Repositories;
using TitanHelp.Application.Interfaces;
using TitanHelp.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure DbContext with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories (Data Access Layer)
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// Register services (Application Layer)
builder.Services.AddScoped<ITicketService, TicketService>();

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default route - starts at Tickets/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tickets}/{action=Index}/{id?}"
);

app.Run();

// Make the Program class accessible for integration testing
public partial class Program { }