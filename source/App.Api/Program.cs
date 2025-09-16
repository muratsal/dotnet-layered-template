using App.Application.Interfaces;
using App.Application.Mapper.Profiles;
using App.Application.Services;
using App.Core.Repository.Interfaces;
using App.Infrastructure;
using App.Infrastructure.Data;
using App.Infrastructure.Repositories;
using App.Web.Common.Security;
using App.Web.Mapper.Account;
using App.Web.Shared.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information().WriteTo.Debug()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddOpenApi();


builder.Services.AddAutoMapper(cfg => { },
    typeof(UserViewModelProfile),
    typeof(UserProfile)
    );



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
  //  options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
    );

var secret = builder.Configuration["JwtSettings:Secret"];
var key = Encoding.UTF8.GetBytes(secret!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("users.read", policy =>
        policy.Requirements.Add(new PermissionRequirement("users.read")));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginService, LoginService>();




var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}

app.MapScalarApiReference();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // MVC route
    endpoints.MapControllerRoute(
        name: "mvc",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // API route
    endpoints.MapControllers();
});

app.Run();
