using Hangfire;
using Hangfire.SqlServer;
using InventoryManagement.Services;
using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Services;
using InventoryManagement_Backend.Services.Interfaces;
//using InventoryManagement_Backend.Services.Interfaces;
using InventoryManagement_Backend.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

//// Add services
//builder.Services.AddControllers()
//    .AddJsonOptions(opts =>
//    {
//        // avoid cycles in JSON output
//        opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
//    });

builder.Services.Configure<StockAlertSettings>(builder.Configuration.GetSection("StockAlert"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();


// DB (SQL Server) - update connection string in appsettings.json
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Register services
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<SupplierCategoryService>();

//builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
// Register JwtSettings for IOptions<>
builder.Services.AddSingleton<IOptions<JwtSettings>>(sp => Options.Create(jwtSettings));

var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddScoped<IPurchaseSalesOrdersService, PurchaseSalesOrdersServices>();



builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IStockAlertService, StockAlertService>();
builder.Services.AddScoped<IInvoiceEmailService, InvoiceEmailService>();

// Hangfire
builder.Services.AddHangfire(config => config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "InventoryOrigin",
        policy => policy.WithOrigins("http://localhost:5046/", "https://localhost:7190").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});


//builder.Services.AddAuthorization();

//builder.Services.AddSwaggerGen(c =>
//{
//    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//        Description = "Enter 'Bearer' followed by your JWT token."
//    });
//    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//    {
//        {
//            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//            {
//                Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                {
//                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            new string[]{}
//        }
//    });
//});

var app = builder.Build();

app.UseCors("InventoryOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHangfireDashboard("/hangfire");

// schedule job
using (var scope = app.Services.CreateScope())
{
    var settings = scope.ServiceProvider.GetRequiredService<IOptions<StockAlertSettings>>().Value;
    RecurringJob.AddOrUpdate<IStockAlertService>(
        "daily-stock-alert",
         s => s.SendDailyLowStockEmailAsync( settings.Threshold),
        Cron.Daily(settings.DailyScheduleHour, settings.DailyScheduleMinute)
        //Cron.Minutely()
        );
}
app.Run();