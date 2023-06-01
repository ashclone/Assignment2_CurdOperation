using Assignment2_FrontEnd.Repository;
using Assignment2_FrontEnd.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentVmRepo, StudentVmRepo>();
builder.Services.AddScoped<T1Interface, StudentVmRepo>();
builder.Services.AddScoped<IHobbiesRepo, HobbyRepo>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
//    options.LoginPath = $"/User/Login";
//    options.LogoutPath = $"/Identity/Account/Logout";

//});



builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
