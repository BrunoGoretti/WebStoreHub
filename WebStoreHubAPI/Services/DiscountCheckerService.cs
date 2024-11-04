using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebStoreHubAPI.Data;
using Microsoft.EntityFrameworkCore;

public class DiscountCheckerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public DiscountCheckerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Fetch discounts that need to be deactivated
                var discountsToDeactivate = await dbContext.Discounts
                    .Where(d => d.IsActive && d.EndDate < DateTime.Now)
                    .ToListAsync(stoppingToken);

                foreach (var discount in discountsToDeactivate)
                {
                    discount.IsActive = false;
                    var product = await dbContext.DbProducts.FirstOrDefaultAsync(p => p.ProductId == discount.ProductId);
                    if (product != null)
                    {
                        product.DiscountedPrice = null;
                        dbContext.DbProducts.Update(product);
                    }
                }

                // Fetch discounts that need to be activated
                var discountsToActivate = await dbContext.Discounts
                    .Where(d => !d.IsActive && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
                    .ToListAsync(stoppingToken);

                foreach (var discount in discountsToActivate)
                {
                    discount.IsActive = true;
                    var product = await dbContext.DbProducts.FirstOrDefaultAsync(p => p.ProductId == discount.ProductId);
                    if (product != null)
                    {
                        decimal newDiscountedPrice = product.Price;

                        // Calculate the discount
                        if (discount.DiscountPercentage > 0)
                        {
                            var discountAmount = product.Price * (discount.DiscountPercentage / 100);
                            newDiscountedPrice -= discountAmount;
                        }

                        if (newDiscountedPrice < 0) newDiscountedPrice = 0;
                        product.DiscountedPrice = newDiscountedPrice;

                        dbContext.DbProducts.Update(product);
                    }
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }

            // Delay before running the next check
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}