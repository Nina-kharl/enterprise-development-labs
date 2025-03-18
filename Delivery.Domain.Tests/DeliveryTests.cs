using Delivery.Domain.Services.InMemory;
using Delivery.Domain.Data;
using Delivery.Domain.Models;
using System.Linq;

namespace Delivery.Domain.Tests;

/// <summary>
/// Класс с юнBooт-тестами репозитория заказов
/// </summary>
public class OrderRepositoryTests
{
    private readonly OrderInMemoryRepository _repository;

    public OrderRepositoryTests()
    {
        // Инициализация репозитория с тестовыми данными
        _repository = new OrderInMemoryRepository();
    }

    /// <summary>
    /// Тест получения заказа по его номеру
    /// </summary>
    [Fact]
    public async Task GetOrderById_Success()
    {
        // Arrange
        var orderId = 1;

        // Act
        var order = await _repository.GetOrderById(orderId);

        // Assert
        Assert.NotNull(order);
        Assert.Equal(orderId, order.Id);
    }

    /// <summary>
    /// Тест получения завершенных заказов, упорядоченных по дате
    /// </summary>
    [Fact]
    public async Task GetCompletedOrdersSortedByDate_Success()
    {
        // Act
        var completedOrders = await _repository.GetCompletedOrdersSortedByDate();

        // Assert
        Assert.True(completedOrders.All(o => o.Status == OrderStatus.Completed));
        Assert.True(completedOrders.SequenceEqual(completedOrders.OrderBy(o => o.OrderDate)));
    }

    /// <summary>
    /// Параметризованный тест получения заказов на указанный день доставки
    /// </summary>
    [Theory]
    [InlineData("2024-03-12", 1)]
    [InlineData("2024-03-13", 1)]
    [InlineData("2024-03-14", 0)]
    public async Task GetOrdersForDeliveryDay_Success(DateTime deliveryDate, int expectedCount)
    {
        // Act
        var orders = await _repository.GetOrdersForDeliveryDay(deliveryDate);

        // Assert
        Assert.Equal(expectedCount, orders.Count);
        Assert.All(orders, o => Assert.Equal(deliveryDate.Date, o.Order.PlannedDeliveryTime.Date));
    }

    /// <summary>
    /// Тест получения количества выполненных заказов для каждого типа товаров
    /// </summary>
    [Fact]
    public async Task GetCompletedOrdersCountByProductType_Success()
    {
        // Act
        var result = await _repository.GetCompletedOrdersCountByProductType();

        // Assert
        Assert.Equal(2, result.Count); // Два типа товаров: Large и Small
        Assert.Contains(result, r => r.ProductType == ProductType.Large && r.Count > 0);
        Assert.Contains(result, r => r.ProductType == ProductType.Small && r.Count > 0);
    }

    /// <summary>
    /// Тест получения курьеров с максимальным числом выполненных заказов
    /// </summary>
    [Fact]
    public async Task GetTopCouriersByCompletedOrders_Success()
    {
        // Act
        var topCouriers = await _repository.GetTopCouriersByCompletedOrders();

        // Assert
        Assert.True(topCouriers.Any());
        Assert.All(topCouriers, c => Assert.True(c.CompletedOrders > 0));
        Assert.True(topCouriers.SequenceEqual(topCouriers.OrderByDescending(c => c.CompletedOrders)));
    }

    /// <summary>
    /// Тест получения опоздавших заказов (≥15 минут)
    /// </summary>
    [Fact]
    public async Task GetDelayedOrders_Success()
    {
        // Act
        var delayedOrders = await _repository.GetDelayedOrders();

        // Assert
        Assert.All(delayedOrders, o =>
        {
            Assert.True(o.Delay.TotalMinutes >= 15);
            Assert.True(o.Order.ActualDeliveryTime > o.Order.PlannedDeliveryTime.AddMinutes(15));
        });
        Assert.True(delayedOrders.SequenceEqual(delayedOrders.OrderByDescending(o => o.Delay)));
    }
}