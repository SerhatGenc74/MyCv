using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MyCv.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));



builder.Services.AddHttpContextAccessor(); 
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RefreshInterval = TimeSpan.FromMinutes(30);
    options.RequireHttpsMetadata = false;
    options.Audience = "localhost:3000";
    options.Authority = "localhost:5000";
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "localhost:5000",
        ValidAudience = "localhost:3000",
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    }; 
    options.MapInboundClaims = false;
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
            return System.Threading.Tasks.Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
            return System.Threading.Tasks.Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowAll");
app.UseSession(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
