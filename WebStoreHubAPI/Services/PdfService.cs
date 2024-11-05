using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using WebStoreHubAPI.Models;

public class PdfService : IPdfService
{
    public byte[] GenerateOrderPdf(OrderModel order)
    {
        using (var stream = new MemoryStream())
        {
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12);

            gfx.DrawString($"Order ID: {order.OrderId}", font, XBrushes.Black, new XPoint(40, 50));
            gfx.DrawString($"Order Date: {order.OrderDate}", font, XBrushes.Black, new XPoint(40, 80));
            gfx.DrawString($"Total Amount: {order.TotalAmount:C}", font, XBrushes.Black, new XPoint(40, 110));

            int yPosition = 150;
            foreach (var item in order.OrderItems)
            {
                gfx.DrawString($"Product: {item.Product.Name}, Quantity: {item.Quantity}, Price: {item.Price:C}", font, XBrushes.Black, new XPoint(40, yPosition));
                yPosition += 30;
            }

            document.Save(stream);
            return stream.ToArray();
        }
    }
}