using ArticleManagement.Web.Helpers;
using ArticleManagement.Web.Middleware;
using ArticleManagement.Web.Services;
using ArticleManagement.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllersWithViews()
    .AddSessionStateTempDataProvider();
builder.Services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
builder.Services.AddHttpClient("ApiClient", client =>
{
  client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromMinutes(30); // tiempo de expiración
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
});
builder.Services.AddAutoMapper(typeof(Program)); // Registra AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
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
app.UseSession(); // debe ir después de UseRouting
app.UseMiddleware<ApiExceptionMiddleware>();
app.UseAuthentication(); // si estás usando auth con JWT o cookies
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
