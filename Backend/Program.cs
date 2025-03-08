using System.Text;
using Config;
using JWT_Token_Example.Carts.CartDataAccess;
using JWT_Token_Example.Context;
using JWT_Token_Example.Controllers;
using JWT_Token_Example.ImageServices;
using JWT_Token_Example.Inventory.InventoryDataAccess;
using JWT_Token_Example.Inventory.InventorySearchAccess;
using JWT_Token_Example.Order.OrderDataAccess;
using JWT_Token_Example.Repository;
using JWT_Token_Example.Reviews.ReviewModels;
using JWT_Token_Example.UtilityService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddTransient<INotification, Notification>();
builder.Services.AddTransient<IUserService,UserService>();
builder.Services.AddTransient<IResetPassword, ResetPassword>();
builder.Services.AddTransient<UserRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<Configuration>();


// Add services to the container.
builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddCookie(x =>
        {
            x.Cookie.Name = "token";
        }
        )
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TPDNmk5ADponVEiQc5tmRkHhOiAFmkAr")),
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero
        };
        x.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["token"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200","https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("JavsConnectionString"));
});

// builder.Services.Configure<CartController>(builder.Configuration.GetSection("MongoDB"));
// builder.Services.AddSingleton<CartController>();

// builder.Services.Configure<CartController>(builder.Configuration.GetSection("MongoDB"));
// builder.Services.AddSingleton<CartController>();

builder.Services.Configure<DataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<DataAccess>();
//
builder.Services.Configure<OrderDataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<OrderDataAccess>();
//
builder.Services.Configure<ReviewDataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<ReviewDataAccess>();
//
builder.Services.Configure<SearchAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<SearchAccess>();

builder.Services.Configure<CartDataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<CartDataAccess>();

builder.Services.AddScoped<IAWSConfiguration, AWSConfiguration>();
builder.Services.AddSingleton<S3Service>();


var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();