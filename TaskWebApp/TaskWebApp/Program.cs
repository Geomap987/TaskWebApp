using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using TaskWebApp.Controllers;
using TaskWebApp.CustomMiddlewares;
using TaskWebApp.DbStuff;
using TaskWebApp.DbStuff.Repositories;
using TaskWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Server=(localdb)\\MSSQLLocalDB; Database=TaskApp; Integrated Security=True";

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
builder.Services.AddScoped<TaskPermissions>();


var app = builder.Build();

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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
