using System.ComponentModel.DataAnnotations;
using Delivery.Domain.Models;
using Delivery.Domain.Data;
using Delivery.Domain.Interfaces;

namespace Delivery.Domain.Services.InMemory;

/// <summary>
/// Репозиторий клиентов с хранением в оперативной памяти
/// </summary>
public class CustomerInMemoryRepository : ICustomerRepository
{
    private List<Customer> _customers;
    private List<Order> _orders;

    /// <summary>
    /// Инициализация данных из сидера
    /// </summary>
    public CustomerInMemoryRepository()
    {
        _customers = DataSeeder.Customers;
        _orders = DataSeeder.Orders;

        // Настраиваем связи между заказами и клиентами
        foreach (var order in _orders)
        {
            order.Customer = _customers.FirstOrDefault(c => c.Id == order.CustomerId);
        }

        foreach (var customer in _customers)
        {
            customer.Orders = _orders.Where(o => o.CustomerId == customer.Id).ToList();
        }
    }

    /// <inheritdoc/>
    public Task<Customer> Add(Customer entity)
    {
        try
        {
            entity.Id = _customers.Max(c => c.Id) + 1;
            _customers.Add(entity);
        }
        catch
        {
            return Task.FromResult<Customer>(null!);
        }
        return Task.FromResult(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(int key)
    {
        try
        {
            var customer = await Get(key);
            if (customer != null)
            {
                _customers.Remove(customer);

                // Каскадное удаление связанных заказов
                var relatedOrders = _orders.Where(o => o.CustomerId == key).ToList();
                foreach (var order in relatedOrders)
                {
                    _orders.Remove(order);
                }
            }
        }
        catch
        {
            return false;
        }
        return true;
    }

    /// <inheritdoc/>
    public async Task<Customer> Update(Customer entity)
    {
        try
        {
            var existing = await Get(entity.Id);
            if (existing != null)
            {
                existing.FirstName = entity.FirstName;
                existing.LastName = entity.LastName;
                existing.Phone = entity.Phone;
                existing.Address = entity.Address;
            }
        }
        catch
        {
            return null!;
        }
        return entity;
    }

    /// <inheritdoc/>
    public Task<Customer?> Get(int key) =>
        Task.FromResult(_customers.FirstOrDefault(c => c.Id == key));

    /// <inheritdoc/>
    public Task<IList<Customer>> GetAll() =>
        Task.FromResult((IList<Customer>)_customers);

    /// <summary>
    /// 1. Получить информацию о заказе по его номеру
    /// </summary>
    public Task<Order?> GetOrderById(int orderId) =>
        Task.FromResult(_orders.FirstOrDefault(o => o.Id == orderId));

    /// <summary>
    /// 2. Получить завершенные заказы, упорядоченные по дате заказа
    /// </summary>
    public Task<IList<Order>> GetCompletedOrdersSortedByDate() =>
        Task.FromResult((IList<Order>)_orders
            .Where(o => o.Status == OrderStatus.Completed)
            .OrderBy(o => o.OrderDate)
            .ToList());

    /// <summary>
    /// 3. Получить заказы и курьеров на указанный день доставки
    /// </summary>
    public Task<IList<(Order Order, Courier? Courier)>> GetOrdersForDeliveryDay(DateTime deliveryDate) =>
        Task.FromResult((IList<(Order Order, Courier? Courier)>)_orders
            .Where(o => o.PlannedDeliveryTime.Date == deliveryDate.Date)
            .Select(o => (o, o.Courier))
            .ToList());

    /// <summary>
    /// 4. Получить количество выполненных заказов для каждого типа товаров
    /// </summary>
    public Task<IList<(ProductType ProductType, int Count)>> GetCompletedOrdersCountByProductType() =>
        Task.FromResult((IList<(ProductType ProductType, int Count)>)_orders
            .Where(o => o.Status == OrderStatus.Completed)
            .GroupBy(o => o.ProductType)
            .Select(g => (g.Key, g.Count()))
            .ToList());

    /// <summary>
    /// 5. Получить курьеров с максимальным числом выполненных заказов
    /// </summary>
    public Task<IList<(Courier Courier, int CompletedOrders)>> GetTopCouriersByCompletedOrders()
    {
        var courierStats = _orders
            .Where(o => o.Status == OrderStatus.Completed && o.CourierId.HasValue)
            .GroupBy(o => o.CourierId)
            .Select(g => new
            {
                CourierId = g.Key,
                CompletedOrders = g.Count()
            })
            .OrderByDescending(cs => cs.CompletedOrders)
            .Take(5)
            .ToList();

        return Task.FromResult((IList<(Courier Courier, int CompletedOrders)>)courierStats
            .Select(cs => (_customers
                .SelectMany(c => c.Orders)
                .FirstOrDefault(o => o.CourierId == cs.CourierId)?.Courier ?? null, cs.CompletedOrders))
            .Where(x => x.Courier != null)
            .ToList());
    }

    /// <summary>
    /// 6. Получить опоздавшие заказы (≥15 минут), упорядоченные по времени задержки
    /// </summary>
    public Task<IList<(Order Order, TimeSpan Delay)>> GetDelayedOrders()
    {
        return Task.FromResult((IList<(Order Order, TimeSpan Delay)>)_orders
            .Where(o =>
                o.Status == OrderStatus.Completed &&
                o.ActualDeliveryTime.HasValue &&
                o.ActualDeliveryTime > o.PlannedDeliveryTime.AddMinutes(15))
            .Select(o => (o, o.ActualDeliveryTime.Value - o.PlannedDeliveryTime))
            .OrderByDescending(x => x.Delay)
            .ToList());
    }
}