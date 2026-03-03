using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TaskWebApp.Controllers;
using TaskWebApp.CustomMiddlewares;
using TaskWebApp.DbStuff;
using TaskWebApp.DbStuff.Models;
using TaskWebApp.DbStuff.Repositories;
using TaskWebApp.DbStuff.Seed;
using TaskWebApp.Services;
using TaskWebApp.Services.ApiServices;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WebDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddAuthentication(AuthController.AUTH_KEY)
    .AddCookie(AuthController.AUTH_KEY, option =>
{
    option.AccessDeniedPath = "/Auth/Deny";
    option.LoginPath = "/Auth/Login";
});

builder.Services.AddDbContext<WebDbContext>(x => x.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<TaskPermissions>();
builder.Services.AddSignalR();
builder.Services.AddHostedService<NewsService>();


builder.Services.AddHttpClient<QuotesApi>(client =>
{
    client.BaseAddress = new Uri("https://zenquotes.io/api/");
});
builder.Services.AddHttpClient<NewsService>();


var app = builder.Build();

var seedEnabled = builder.Configuration.GetValue<bool?>("DemoSeed:Enabled")
                 ?? app.Environment.IsDevelopment();

if (seedEnabled)
{
    await DemoUserSeed.SeedAsync(app.Services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CustomLocalizationMiddleware>();

app.MapHub<NewsHub>("/newsHub");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
