using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Data;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

public class BrandService : IBrandService
{
    private readonly AppDbContext _dbContext;

    public BrandService(AppDbContext context)
    {
        _dbContext = context;
    }

    public async Task<BrandModel> CreateBrandAsync(BrandModel brand)
    {
        _dbContext.DbBrands.Add(brand);
        await _dbContext.SaveChangesAsync();
        return brand;
    }
    public async Task<BrandModel> GetBrandByIdAsync(int brandId)
    {
        return await _dbContext.DbBrands.FirstOrDefaultAsync(p => p. BrandId == brandId);
    }

    public async Task<IEnumerable<BrandModel>> GetAllBrandsAsync()
    {
        return await _dbContext.DbBrands.ToListAsync();
    }

    public async Task<BrandModel> UpdateBrandAsync(int brandId, string brandName)
    {
        var existingBrand = await _dbContext.DbBrands.FirstOrDefaultAsync(p => p.BrandId == brandId);
        if (existingBrand == null)
        {
            return null;
        }

        existingBrand.BrandId = brandId;
        existingBrand.BrandName = brandName;


        await _dbContext.SaveChangesAsync();
        return existingBrand;
    }

    public async Task<bool> DeleteBrandAsync(int brandId)
    {
        var existingBrand = await _dbContext.DbBrands.FindAsync(brandId);
        if (existingBrand == null)
        {
            return false;
        }

        _dbContext.DbBrands.Remove(existingBrand);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}