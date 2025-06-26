using GRCAuthServicesMVC.Models;
using GRCAuthServicesMVC.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using GRCAuthServicesMVC.Mapping;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GRCAuthServicesMVC.Data;
using Microsoft.AspNetCore.Identity;
using GRCAuthServicesMVC.Controllers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContext<AuthDbcontext>(options => 
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultAuth")));

builder.Services.AddDbContext<AuthDbcontext>(options =>
    options.UseInMemoryDatabase("InMemoryIdentityDb"));


builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbcontext>()
    .AddDefaultTokenProviders();


//builder.Services.AddIdentityCore<IdentityUser>()
//    .AddRoles<IdentityRole>()
//    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("GRCAuthServicesMVC")
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie();

builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Home/AccessDenied"; 
        options.AccessDeniedPath = "/Home/AccessDenied";
    });


//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequiredLength = 6;
//});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audiance"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    });

builder.Services.AddScoped<IApplicationInventoryRepository, SqlApplicationInventoryRepository>();
builder.Services.AddScoped<IApplicationRole, SQLApplicationRole>();
builder.Services.AddScoped<ITokenRepo, TokenRepository>();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbSeeder.SeedRoles(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
