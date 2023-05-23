using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using Blazelounge_v2.Repositories;
using Blazelounge_v2.Services;
using Blazelounge_v2.Data.Models;
using Microsoft.EntityFrameworkCore;
using Blazelounge_v2.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Configuration
builder.Configuration.AddJsonFile("appsettings.json");

// Register additional services
// Replace the service registrations with your own services
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(options =>
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes
        .Concat(new[] { "application/octet-stream" })
);

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "blaze-lounge-API",
        Description = "API",
        Version = "v1"
    });
});

// Add Dependency injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddSingleton<CrashGameStateService>();

//// Add SQLite DbContext
builder.Services.AddDbContext<DbContext2>(options =>
{
    options.UseSqlite("Data Source=db.db");
});

// Add Cors
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAllOrigins",
//        builder => builder.AllowAnyOrigin()
//                          .AllowAnyMethod()
//                          .AllowAnyHeader()
//                          .WithExposedHeaders("content-disposition") // Add this line
//                          .WithExposedHeaders("content-type") // Add this line
//                          .WithExposedHeaders("X-Pagination") // Add this line
//                          .WithExposedHeaders("X-Total-Count")); // Add this line
//});

var app = builder.Build();

//add middleware for chat
app.UseResponseCompression();

// Add Hub mapping
app.MapHub<ChatHub>("/chathub");
app.MapHub<CrashHub>("/crashhub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseCors(options => options.AllowAnyOrigin());
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "blaze-lounge-API v1");
    });
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

await app.RunAsync();
