using Assignment2_CurdOperation.Data;
using Microsoft.EntityFrameworkCore;
using Assignment2_CurdOperation.Controllers;
using Assignment2_CurdOperation.Repository.Interface;
using Assignment2_CurdOperation.Repository;
using Assignment2_CurdOperation.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using Assignment2_CurdOperation.Modals;
using Assignment2_CurdOperation;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("conStr"),
b =>b.MigrationsAssembly("Assignment2_CurdOperation")));

builder.Services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
builder.Services.AddTransient<UserManager<ApplicationUser>, ApplicationUserManager>();
builder.Services.AddTransient<SignInManager<ApplicationUser>, ApplicationSignInManager>();
builder.Services.AddTransient<RoleManager<ApplicationRole>, ApplicationRoleManager>();
builder.Services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
builder.Services.AddTransient<IUserService, UserServiceRepository>();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddUserStore<ApplicationUserStore>()
.AddUserManager<ApplicationUserManager>()
.AddRoleManager<ApplicationRoleManager>()
.AddSignInManager<ApplicationSignInManager>()
.AddRoleStore<ApplicationRoleStore>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<ApplicationRoleStore>();
builder.Services.AddScoped<ApplicationUserStore>();


builder.Services.AddScoped<IStudentRespository, StudentRepository>();
builder.Services.AddScoped<IHobbyRepository,HobbyRepository>();
builder.Services.AddScoped<IStudentVmRepository,StudentVmRepository>();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigurationSwaggerOption>();

builder.Services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),
                    "./ServerFiles")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//jwt
var appSettingSection = builder.Configuration.GetSection("jwtAppSetting");
builder.Services.Configure<JwtAppSettingSecretKey>(appSettingSection);
var appSetting = appSettingSection.Get<JwtAppSettingSecretKey>();
var key = Encoding.ASCII.GetBytes(appSetting.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();

//IServiceScopeFactory serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
//using (IServiceScope scope = serviceScopeFactory.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//    var role = new ApplicationRole() { Name = "Admin" };
//    await roleManager.CreateAsync(role);
//    var role2 = new ApplicationRole() { Name = "Employee" };
//    await roleManager.CreateAsync(role2);
//    var user = new ApplicationUser();
//    user.UserName = "admin@gmail.com";
//    user.Email = "admin@gmail.com";
//    var UserPassword = "Admin@123";
//    await userManager.CreateAsync(user, UserPassword);
//    await userManager.AddToRoleAsync(user, "Admin");
//    var employee = new ApplicationUser();
//    employee.UserName = "employee@gmail.com";
//    employee.Email = "employee@gmail.com";
//    var UserPassword2 = "Admin@123";
//    await userManager.CreateAsync(employee, UserPassword2);
//    await userManager.AddToRoleAsync(employee, "Employee");
//}

app.MapControllers();


app.Run();
