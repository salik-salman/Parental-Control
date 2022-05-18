using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parental_Control.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Parental_ControlContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Parental_ControlContext") ?? throw new InvalidOperationException("Connection string 'Parental_ControlContext' not found.")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30);//We set Time here 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
