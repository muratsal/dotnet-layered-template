using App.Application.Interfaces;
using App.Application.Mapper.Profiles;
using App.Application.Services;
using App.Core.Repository.Interfaces;
using App.Infrastructure;
using App.Infrastructure.Data;
using App.Infrastructure.Repositories;
using App.Web.Mapper.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using System;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();




var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}

//app.UseReDoc(options =>
//{
//    options.SpecUrl("/openapi/v1.json");
//});

app.MapScalarApiReference();

//app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

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
