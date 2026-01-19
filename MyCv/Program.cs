using Microsoft.EntityFrameworkCore;
using MyCv.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

Console.WriteLine("ENV: " + builder.Environment.EnvironmentName);
Console.WriteLine("CS: " + builder.Configuration.GetConnectionString("conn"));

builder.Services.AddHttpContextAccessor(); // HttpContext eriþimi için
builder.Services.AddDistributedMemoryCache(); // session cache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1); // session süresi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


var app = builder.Build();

app.UseCors("AllowAll");
app.UseSession(); // Middleware’i eklemeyi unutma

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
