using System.ComponentModel.DataAnnotations;
using Delivery.Domain.Models;
using Delivery.Domain.Data;
using Delivery.Domain.Interfaces;

namespace Delivery.Domain.Services.InMemory;

/// <summary>
/// Репозиторий позиций заказов с хранением в оперативной памяти
/// </summary>
public class OrderItemInMemoryRepository : IRepository<OrderItem, int>
{
    private List<OrderItem> _orderItems;
    private List<Order> _orders;
    private List<Product> _products;

    /// <summary>
    /// Инициализация данных из сидера
    /// </summary>
    public OrderItemInMemoryRepository()
    {
        _orderItems = DataSeeder.OrderItems;
        _orders = DataSeeder.Orders;
        _products = DataSeeder.Products;

        // Настраиваем связи
        foreach (var item in _orderItems)
        {
            item.Order = _orders.FirstOrDefault(o => o.Id == item.OrderId);
            item.Product = _products.FirstOrDefault(p => p.Id == item.ProductId);
        }
    }

    /// <inheritdoc/>
    public Task<OrderItem> Add(OrderItem entity)
    {
        try
        {
            entity.Id = _orderItems.Max(oi => oi.Id) + 1;
            _orderItems.Add(entity);
        }
        catch
        {
            return Task.FromResult<OrderItem>(null!);
        }
        return Task.FromResult(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(int key)
    {
        try
        {
            var item = await Get(key);
            if (item != null)
                _orderItems.Remove(item);
        }
        catch
        {
            return false;
        }
        return true;
    }

    /// <inheritdoc/>
    public Task<OrderItem?> Get(int key) =>
        Task.FromResult(_orderItems.FirstOrDefault(oi => oi.Id == key));

    /// <inheritdoc/>
    public Task<IList<OrderItem>> GetAll() =>
        Task.FromResult((IList<OrderItem>)_orderItems);

    /// <inheritdoc/>
    public async Task<OrderItem> Update(OrderItem entity)
    {
        try
        {
            var existing = await Get(entity.Id);
            if (existing != null)
            {
                existing.Quantity = entity.Quantity;
                existing.UnitPrice = entity.UnitPrice;
                existing.ProductId = entity.ProductId;
                existing.OrderId = entity.OrderId;
            }
        }
        catch
        {
            return null!;
        }
        return entity;
    }

    /// <summary>
    /// Получить позиции заказа по ID заказа
    /// </summary>
    public Task<IList<OrderItem>> GetByOrderId(int orderId) =>
        Task.FromResult((IList<OrderItem>)_orderItems
            .Where(oi => oi.OrderId == orderId)
            .ToList());

    /// <summary>
    /// Получить позиции с крупногабаритными товарами
    /// </summary>
    public Task<IList<OrderItem>> GetLargeProductItems() =>
        Task.FromResult((IList<OrderItem>)_orderItems
            .Where(oi => oi.Product?.ProductType == ProductType.Large)
            .ToList());
}
