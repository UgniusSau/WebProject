using BlazeLounge.Repositories.Users;
using BlazeLounge.Repositories.Shop;
using BlazeLounge.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using BlazeLounge.Data.Models;
using Microsoft.AspNetCore.ResponseCompression;
using BlazeLounge.Server.Hubs;
using BlazeLounge.Services.Shop;
using BlazeLounge.Services.Crash;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(options =>
	options.MimeTypes = ResponseCompressionDefaults
    .MimeTypes
    .Concat(new[] { "application/octet-stream" })
);

builder.Services.AddServerSideBlazor();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
builder.Services.AddScoped<IDBUserRepository, DBUserRepository>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();


builder.Services.AddSingleton<CrashGameStateService>();



// in memory db
//builder.Services.AddDbContext<ApiDataContext>(options =>
//{
//    //options.UseInMemoryDatabase("MemoryDb");
//});

//// Add SQLite DbContext
builder.Services.AddDbContext<DbContext2>(options =>
{
    options.UseSqlite("Data Source=db.db");
});



// Add Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithExposedHeaders("content-disposition") // Add this line
                          .WithExposedHeaders("content-type") // Add this line
                          .WithExposedHeaders("X-Pagination") // Add this line
                          .WithExposedHeaders("X-Total-Count")); // Add this line
});


var app = builder.Build();

//add middleware for chat
app.UseResponseCompression();

// Add Hub mapping
app.MapHub<ChatHub>("/chathub");
app.MapHub<CrashHub>("/crashhub");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(options => options.AllowAnyOrigin());
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "blaze-lounge-API v1");
    });
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseRouting();

app.UseCors("AllowAllOrigins");

// Add endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.UseStaticFiles();


// Run the application.
app.Run();
