
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options=>
options.IdleTimeout = TimeSpan.FromDays(15));
builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseSession();
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

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
                       name: "Doctor",
                       areaName: "Doctor",
                       pattern: "Doctor/{controller=Doctor}/{action=DoctorProfileList}");

    endpoints.MapAreaControllerRoute(
                       name: "Patient",
                       areaName: "Patient",
                       pattern: "Patient/{controller=Patient}/{action=AppoinmnetList}");

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

});

app.Run();
