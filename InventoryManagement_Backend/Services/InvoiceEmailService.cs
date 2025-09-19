using Hangfire.Server;
using InventoryManagement_Backend.Models;
using System.Text;

namespace InventoryManagement_Backend.Services
{
    public class InvoiceEmailService: IInvoiceEmailService
    {
    private readonly IEmailSender _emailService;

        public InvoiceEmailService(IEmailSender emailService)
        {
            _emailService = emailService;
        }
       public async Task SendOrderInvoiceEmailAsync(User? user, Supplier? supplier, List<PurchaseSalesOrders> orders, Transaction transaction, List<Product> products)
        {
            string Subject = $"Invoice for Transaction #{transaction.TransactionId}";
            string recipientEmail = user?.EmailID ?? supplier?.EmailID;
            string recipientName = user?.Name ?? supplier?.Name;

            if (string.IsNullOrEmpty(recipientEmail))
                throw new Exception("No recipient email found.");

            string Body = GenerateInvoiceHtml(recipientName, orders, transaction, products);

            List<string> ToEmail = new List<string>();
            ToEmail.Add(recipientEmail);
            await _emailService.SendEmailAsync(Subject, Body, ToEmail);
        }

        private string GenerateInvoiceHtml(string recipientName, List<PurchaseSalesOrders> orders, Transaction transaction, List<Product> products)
        {
            var rows = new StringBuilder();
            foreach (var order in orders)
            {
                var product = products.FirstOrDefault(p => p.ProductId == order.ProductId);
                rows.Append($@"
                <tr>
                    <td>{product?.Name ?? "Unknown"}</td>
                    <td>{order.Quantity}</td>
                    <td>{order.TotalAmount:C}</td>
                </tr>");
            }
                    //<td>{(product?.Price ?? 0):C}</td>
                    //< th > Price </ th >

            var grandTotal = orders.Sum(o => o.TotalAmount);

            return $@"
<!DOCTYPE html>
<html>
<head>
  <style>
    body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; }}
    .invoice-container {{ max-width: 600px; margin: auto; background: #fff; padding: 20px; border-radius: 10px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }}
    h2 {{ color: #333; text-align: center; }}
    table {{ width: 100%; border-collapse: collapse; margin-top: 20px; }}
    table, th, td {{ border: 1px solid #ddd; }}
    th, td {{ padding: 10px; text-align: left; }}
    th {{ background-color: #f8f8f8; }}
    .footer {{ margin-top: 20px; font-size: 12px; text-align: center; color: #777; }}
  </style>
</head>
<body>
  <div class='invoice-container'>
    <h2>Order Invoice</h2>
    <p><strong>Customer/Supplier:</strong> {recipientName}</p>
    <p><strong>Transaction ID:</strong> {transaction.TransactionId}</p>
    <p><strong>Date:</strong> {orders.First().OrderDate:yyyy-MM-dd HH:mm}</p>

    <table>
      <thead>
        <tr>
          <th>Product</th>
          <th>Quantity</th>
          
          <th>Total</th>
        </tr>
      </thead>
      <tbody>
        {rows}
      </tbody>
    </table>

    <h3 style='text-align:right;'>Grand Total: {grandTotal:C}</h3>

    <div class='footer'>
      <p>Thank you for shopping with us!</p>
      <p>Inventory Management System</p>
    </div>
  </div>
</body>
</html>";
        }
    }
}
