public class Order
{
    // ... другие свойства

    /// <summary>
    /// Список товаров в заказе
    /// </summary>
    public virtual List<OrderItem> Items { get; set; } = new();

    /// <summary>
    /// Общая стоимость заказа
    /// </summary>
    public decimal TotalAmount =>
        Items.Sum(i => i.Quantity * i.UnitPrice);
}