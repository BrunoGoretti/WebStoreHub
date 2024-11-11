using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using WebStoreHubAPI.Data;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;

        public OrderService(AppDbContext dbContext, IProductService productService, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _productService = productService;
            _configuration = configuration;

        }

        public async Task<OrderModel> CreateOrderAsync(int userId, List<CartItemModel> cartItems)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var totalAmount = cartItems.Sum(c =>
                        (c.Product.DiscountedPrice ?? c.Product.Price) * c.Quantity);

                    var order = new OrderModel
                    {
                        UserId = userId,
                        OrderDate = DateTime.Now,
                        TotalAmount = totalAmount,
                        Status = "Pending",
                        OrderItems = cartItems.Select(c => new OrderItemModel
                        {
                            ProductId = c.ProductId,
                            Quantity = c.Quantity,
                            Price = c.Product.DiscountedPrice ?? c.Product.Price
                        }).ToList()
                    };

                    var paymentSuccess = SimulatePayment(totalAmount);
                    if (!paymentSuccess)
                    {
                        throw new InvalidOperationException("Payment failed.");
                    }

                    foreach (var cartItem in cartItems)
                    {
                        var product = await _productService.GetProductByIdAsync(cartItem.ProductId);
                        if (product.Stock >= cartItem.Quantity)
                        {
                            product.Stock -= cartItem.Quantity;
                            await _productService.UpdateProductAsync(product.ProductId, product.Name, product.Description, product.Price, product.Stock);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
                        }

                    }

                    await _dbContext.Orders.AddAsync(order);
                    await _dbContext.SaveChangesAsync();

                    _dbContext.CartItems.RemoveRange(cartItems);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    order.Status = "Confirmed";
                    await _dbContext.SaveChangesAsync();

                    var pdfBytes = GenerateOrderPdf(order);
                    var userEmail = await GetUserEmailByIdAsync(userId);
                    await SendOrderConfirmationEmail(userEmail, order, pdfBytes);

                    return order;
                }
                catch (Exception ex)
                {
                    // If any exception occurs, rollback the transaction
                    await transaction.RollbackAsync();
                    throw new InvalidOperationException($"Order creation failed: {ex.Message}");
                }
            }
        }

        private async Task<string> GetUserEmailByIdAsync(int userId)
        {
            var user = await _dbContext.DbUsers.FindAsync(userId);
            return user?.Email;
        }

        // Simulate payment (placeholder method)
        private bool SimulatePayment(decimal totalAmount)
        {
            // Simulate a payment success
            return true;
        }

        public async Task<OrderModel> GetOrderByIdAsync(int orderId)
        {
            return await _dbContext.Orders
                   .Include(o => o.OrderItems)
                       .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.ProductType)
                   .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByUserIdAsync(int userId)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.ProductType)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return false;
            }

            order.Status = newStatus;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private byte[] GenerateOrderPdf(OrderModel order)
        {
            using (var document = new PdfDocument())
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Verdana", 12);

                gfx.DrawString("Order Confirmation", font, XBrushes.Black, new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);
                gfx.DrawString($"Order ID: {order.OrderId}", font, XBrushes.Black, new XPoint(20, 60));
                gfx.DrawString($"Date: {order.OrderDate}", font, XBrushes.Black, new XPoint(20, 80));

                int yPosition = 100;
                foreach (var item in order.OrderItems)
                {
                    gfx.DrawString($"{item.Product.Name} - Quantity: {item.Quantity} - Price: ${item.Price}", font, XBrushes.Black, new XPoint(20, yPosition));
                    yPosition += 20;
                }

                gfx.DrawString($"Total Amount: ${order.TotalAmount}", font, XBrushes.Black, new XPoint(20, yPosition + 20));

                using (var stream = new MemoryStream())
                {
                    document.Save(stream, false);
                    return stream.ToArray();
                }
            }
        }

        private async Task SendOrderConfirmationEmail(string email, OrderModel order, byte[] pdfBytes)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpEmail = _configuration["EmailSettings:EmailAddress"];
            var smtpPassword = _configuration["EmailSettings:EmailPassword"];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("WebStoreHub", smtpEmail));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Order Confirmation";

            var body = new TextPart("plain")
            {
                Text = $"Thank you for your order!"
            };

            var pdfAttachment = new MimePart("application", "pdf")
            {
                Content = new MimeContent(new MemoryStream(pdfBytes)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = $"Order_{order.OrderId}.pdf"
            };

            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(pdfAttachment);
            message.Body = multipart;

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpEmail, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
