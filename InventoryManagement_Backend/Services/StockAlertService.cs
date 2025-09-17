using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Models;
using InventoryManagement_Backend.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text;

namespace InventoryManagement_Backend.Services
{
    public class StockAlertService : IStockAlertService
    {
        private readonly InventoryDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly StockAlertSettings _stockSettings;

        public StockAlertService(InventoryDbContext context, IEmailSender emailSender, IOptions<StockAlertSettings> stockSettings)
        {
            _context = context;
            _emailSender = emailSender;
            _stockSettings = stockSettings.Value;
        }

        public async Task<List<Product>> GetLowStockProductsAsync(int threshold)
        {
            return await _context.Products.Where(p => p.Quantity < threshold).ToListAsync();
            //var products = new List<Product>
            //    {
            //        new Product { Name = "Laptop", Category = "Electronics", Description = "Dell Laptop", Quantity = 3, SupplierId = 1 },
            //        new Product { Name = "Keyboard", Category = "Electronics", Description = "Mechanical Keyboard", Quantity = 15, SupplierId = 1 },
            //        new Product { Name = "Mouse", Category = "Electronics", Description = "Wireless Mouse", Quantity = 2, SupplierId = 2 },
            //        new Product { Name = "Notebook", Category = "Stationery", Description = "A4 Notebook", Quantity = 50, SupplierId = 2 },
            //        new Product { Name = "Pen", Category = "Stationery", Description = "Gel Pen", Quantity = 5, SupplierId = 3 }
            //    };

            //products.AddRange(products);
            //return products.Where(p => p.Quantity < threshold).ToList();
        }

        public async Task SendLowStockEmailAsync(List<Product> lowstockProducts)
        {
            if (lowstockProducts == null || !lowstockProducts.Any())
                return;

            var sb = new StringBuilder();

            // Build email body
            sb.Append(@"<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Low Stock Alert</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
            margin: 0;
            padding: 20px;
        }
        .email-container {
            max-width: 600px;
            margin: auto;
            background: #ffffff;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0px 4px 12px rgba(0,0,0,0.1);
        }
        .header {
            background: #007BFF;
            color: #ffffff;
            text-align: center;
            padding: 20px;
        }
        .header h2 {
            margin: 0;
        }
        .content {
            padding: 20px;
        }
       table {
            width: 100%;
            border-collapse: collapse;
            min-width: 500px; /* ensures scroll triggers on small screens */
            }
        table th, table td {
            border: 1px solid #dddddd;
             text-align: left;
            padding: 12px;
             white-space: nowrap; /* prevents text wrapping */
            }
        table th {
            background: #007BFF;
            color: #ffffff;
        }
        table tr:nth-child(even) {
            background-color: #f2f2f2;
        }
        .footer {
            text-align: center;
            padding: 15px;
            background: #f1f1f1;
            font-size: 12px;
            color: #555555;
        }
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='header'>
            <h2>Low Stock Alert 🚨</h2>
        </div>
        <div class='content'>
            <p>Hello Team,</p>
            <p>The following products are running low on stock:</p>
              <div style='overflow-x:auto;'>
            <table>
                <thead>
                    <tr>
                        <th>Product Name</th>
                        <th>Category</th>
                        <th>Quantity</th>
                        <th>Supplier</th>
                    </tr>
                </thead>
                <tbody>");

            // Add product rows dynamically
            foreach (var product in lowstockProducts)
            {
                sb.Append($@"
                    <tr>
                        <td>{product.Name}</td>
                        <td>{product.Category ?? "N/A"}</td>
                        <td>{product.Quantity}</td>
                        <td>{product.Supplier?.Name ?? "Unknown"}</td>
                    </tr>");
            }

            sb.Append(@"
                </tbody>
            </table>
            </div>
            <p>Please take necessary action to restock these items.</p>
        </div>
        <div class='footer'>
            <p>&copy; 2025 Inventory Management System. All rights reserved.</p>
        </div>
    </div>
</body>
</html>");

            string body = sb.ToString();

            await _emailSender.SendEmailAsync("Daily Stock Alert", body, _stockSettings.Recipients);
        }

        public async Task SendDailyLowStockEmailAsync(int threshold)
        {
            var lowStockProducts = await GetLowStockProductsAsync(threshold);
            await SendLowStockEmailAsync(lowStockProducts);
        }


    }
}
