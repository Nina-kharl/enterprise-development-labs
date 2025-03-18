using System.ComponentModel.DataAnnotations;
using Delivery.Domain.Models;
using Delivery.Domain.Data;
using Delivery.Domain.Interfaces;

namespace Delivery.Domain.Services.InMemory;

/// <summary>
/// Репозиторий товаров с хранением в оперативной памяти
/// </summary>
public class ProductInMemoryRepository : IRepository<Product, int>
{
    private List<Product> _products;

    /// <summary>
    /// Инициализация данных из сидера
    /// </summary>
    public ProductInMemoryRepository()
    {
        _products = DataSeeder.Products;
    }

    /// <inheritdoc/>
    public Task<Product> Add(Product entity)
    {
        try
        {
            entity.Id = _products.Max(p => p.Id) + 1;
            _products.Add(entity);
        }
        catch
        {
            return Task.FromResult<Product>(null!);
        }
        return Task.FromResult(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(int key)
    {
        try
        {
            var product = await Get(key);
            if (product != null)
                _products.Remove(product);
        }
        catch
        {
            return false;
        }
        return true;
    }

    /// <inheritdoc/>
    public Task<Product?> Get(int key) =>
        Task.FromResult(_products.FirstOrDefault(p => p.Id == key));

    /// <inheritdoc/>
    public Task<IList<Product>> GetAll() =>
        Task.FromResult((IList<Product>)_products);

    /// <inheritdoc/>
    public async Task<Product> Update(Product entity)
    {
        try
        {
            var existing = await Get(entity.Id);
            if (existing != null)
            {
                existing.Name = entity.Name;
                existing.Description = entity.Description;
                existing.Weight = entity.Weight;
                existing.Dimensions = entity.Dimensions;
                existing.SKU = entity.SKU;
                existing.ProductType = entity.ProductType;
            }
        }
        catch
        {
            return null!;
        }
        return entity;
    }

    /// <summary>
    /// Поиск товаров по типу
    /// </summary>
    public Task<IList<Product>> GetByType(ProductType type) =>
        Task.FromResult((IList<Product>)_products
            .Where(p => p.ProductType == type)
            .ToList());

    /// <summary>
    /// Поиск крупногабаритных товаров
    /// </summary>
    public Task<IList<Product>> GetLargeProducts() =>
        Task.FromResult((IList<Product>)_products
            .Where(p => p.ProductType == ProductType.Large)
            .ToList());
}