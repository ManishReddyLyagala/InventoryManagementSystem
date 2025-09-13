using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        // avoid cycles in JSON output
        opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB (SQL Server) - update connection string in appsettings.json
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Register services
//builder.Services.AddScoped<ISupplierService, SupplierService>();
//builder.Services.AddScoped<ICustomerService, CustomerService>();
//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IPurchaseSalesOrdersService, PurchaseSalesOrdersServices>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "InventoryOrigin",
        policy => policy.WithOrigins("http://localhost:5046/", "https://localhost:7190").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
var app = builder.Build();

app.UseCors("InventoryOrigin");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
