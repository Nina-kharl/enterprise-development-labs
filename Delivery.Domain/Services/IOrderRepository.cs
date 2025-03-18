public class DeliveryService
{
    private readonly IOrderRepository _orderRepository;

    public DeliveryService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    // Пример: Получить завершенные заказы
    public async Task PrintCompletedOrders()
    {
        var completedOrders = await _orderRepository.GetCompletedOrdersSortedByDate();
        foreach (var order in completedOrders)
        {
            Console.WriteLine($"Заказ #{order.Id}, Дата: {order.OrderDate}");
        }
    }

    // Пример: Получить топ-курьеров
    public async Task PrintTopCouriers()
    {
        var topCouriers = await _orderRepository.GetTopCouriersByCompletedOrders();
        foreach (var (courier, completedOrders) in topCouriers)
        {
            Console.WriteLine($"{courier.FullName}: {completedOrders} выполненных заказов");
        }
    }

    // Пример: Получить опоздавшие заказы
    public async Task PrintDelayedOrders()
    {
        var delayedOrders = await _orderRepository.GetDelayedOrders();
        foreach (var (order, delay) in delayedOrders)
        {
            Console.WriteLine($"Заказ #{order.Id}: задержка {delay.TotalMinutes} минут");
        }
    }
}