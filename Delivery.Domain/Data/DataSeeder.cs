using Delivery.Domain.Models;

namespace Delivery.Domain.Data;

public static class DataSeeder
{
    /// <summary>
    /// Список клиентов
    /// </summary>
    public static readonly List<Customer> Customers =
    [
        new()
        {
            Id = 1,
            FullName = "Иванов Иван Иванович",
            Address = "ул. Ленина, 15",
            Phone = "+79111234567"
        },
        new()
        {
            Id = 2,
            FullName = "Петрова Анна Сергеевна",
            Address = "пр. Карла Маркса, 34",
            Phone = "+79219876543"
        }
    ];

    /// <summary>
    /// Список транспортных средств
    /// </summary>
    public static readonly List<Vehicle> Vehicles =
    [
        new()
        {
            Id = 1,
            LicensePlate = "А123ВС78",
            Model = "Газель",
            CanCarryLarge = true,
            CourierId = 1
        },
        new()
        {
            Id = 2,
            LicensePlate = "М789ОР199",
            Model = "Ford Transit",
            CanCarryLarge = false,
            CourierId = 2
        }
    ];

    /// <summary>
    /// Список курьеров
    /// </summary>
    public static readonly List<Courier> Couriers =
    [
        new()
        {
            Id = 1,
            FullName = "Сидоров Петр Владимирович",
            Phone = "+79581112233",
            Vehicle = Vehicles[0]
        },
        new()
        {
            Id = 2,
            FullName = "Кузнецов Дмитрий Алексеевич",
            Phone = "+79582223344",
            Vehicle = Vehicles[1]
        }
    ];

    /// <summary>
    /// Список заказов
    /// </summary>
    public static readonly List<Order> Orders =
    [
        new()
        {
            Id = 1,
            OrderDate = new DateTime(2024, 3, 10),
            ProductType = ProductType.Large,
            Status = OrderStatus.Completed,
            PlannedDeliveryTime = new DateTime(2024, 3, 12, 10, 0, 0),
            ActualDeliveryTime = new DateTime(2024, 3, 12, 10, 5, 0),
            CustomerId = 1,
            CourierId = 1
        },
        new()
        {
            Id = 2,
            OrderDate = new DateTime(2024, 3, 11),
            ProductType = ProductType.Small,
            Status = OrderStatus.Completed,
            PlannedDeliveryTime = new DateTime(2024, 3, 13, 14, 0, 0),
            ActualDeliveryTime = new DateTime(2024, 3, 13, 14, 20, 0),
            CustomerId = 2,
            CourierId = 2
        },
        new()
        {
            Id = 3,
            OrderDate = new DateTime(2024, 3, 12),
            ProductType = ProductType.Large,
            Status = OrderStatus.InProgress,
            PlannedDeliveryTime = new DateTime(2024, 3, 14, 16, 0, 0),
            CustomerId = 1,
            CourierId = 1
        },
        new()
        {
            Id = 4,
            OrderDate = new DateTime(2024, 3, 13),
            ProductType = ProductType.Small,
            Status = OrderStatus.Canceled,
            PlannedDeliveryTime = new DateTime(2024, 3, 15, 9, 0, 0),
            CustomerId = 2
        },
        new()
        {
            Id = 5,
            OrderDate = new DateTime(2024, 3, 14),
            ProductType = ProductType.Large,
            Status = OrderStatus.Completed,
            PlannedDeliveryTime = new DateTime(2024, 3, 16, 11, 0, 0),
            ActualDeliveryTime = new DateTime(2024, 3, 16, 11, 30, 0),
            CustomerId = 1,
            CourierId = 1
        }
    ];

    /// <summary>
    /// Список товаров
    /// </summary>
    public static readonly List<Product> Products =
    [
        new()
        {
            Id = 1,
            Name = "Холодильник",
            Description = "Большой двухкамерный холодильник",
            Weight = 50,
            Dimensions = "180x70x70",
            SKU = "FRIDGE001",
            ProductType = ProductType.Large
        },
        new()
        {
            Id = 2,
            Name = "Микроволновая печь",
            Description = "Компактная микроволновая печь",
            Weight = 10,
            Dimensions = "30x50x40",
            SKU = "MICROWAVE001",
            ProductType = ProductType.Small
        },
        new()
        {
            Id = 3,
            Name = "Стол обеденный",
            Description = "Деревянный обеденный стол",
            Weight = 30,
            Dimensions = "150x90x75",
            SKU = "TABLE001",
            ProductType = ProductType.Large
        },
        new()
        {
            Id = 4,
            Name = "Чайник электрический",
            Description = "Электрический чайник объемом 1.7 литра",
            Weight = 2,
            Dimensions = "20x20x30",
            SKU = "KETTLE001",
            ProductType = ProductType.Small
        }
    ];

    /// <summary>
    /// Список позиций заказа (связь заказов и товаров)
    /// </summary>
    public static readonly List<OrderItem> OrderItems =
    [
        new()
        {
            Id = 1,
            OrderId = 1,
            ProductId = 1,
            Quantity = 1,
            UnitPrice = 30000
        },
        new()
        {
            Id = 2,
            OrderId = 2,
            ProductId = 2,
            Quantity = 2,
            UnitPrice = 5000
        },
        new()
        {
            Id = 3,
            OrderId = 3,
            ProductId = 3,
            Quantity = 1,
            UnitPrice = 15000
        },
        new()
        {
            Id = 4,
            OrderId = 4,
            ProductId = 4,
            Quantity = 3,
            UnitPrice = 2000
        },
        new()
        {
            Id = 5,
            OrderId = 5,
            ProductId = 1,
            Quantity = 1,
            UnitPrice = 30000
        }
    ];
}