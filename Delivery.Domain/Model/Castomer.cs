using System.ComponentModel.DataAnnotations;

namespace Delivery.Domain.Models;

/// <summary>
/// Клиент службы доставки
/// </summary>
public class Customer
{
    /// <summary>
    /// Уникальный идентификатор клиента
    /// </summary>
    [Key]
    public required int Id { get; set; }

    /// <summary>
    /// Фамилия клиента
    /// </summary>
    [Required(ErrorMessage = "Фамилия обязательна")]
    public string LastName { get; set; }

    /// <summary>
    /// Имя клиента
    /// </summary>
    [Required(ErrorMessage = "Имя обязательно")]
    public string FirstName { get; set; }

    /// <summary>
    /// Отчество клиента
    /// </summary>
    public string? Patronymic { get; set; }

    /// <summary>
    /// Контактный телефон
    /// </summary>
    [Phone]
    [Required(ErrorMessage = "Телефон обязателен")]
    public string Phone { get; set; }

    /// <summary>
    /// Адрес доставки
    /// </summary>
    [Required(ErrorMessage = "Адрес обязателен")]
    public string Address { get; set; }

    /// <summary>
    /// Список заказов клиента
    /// </summary>
    public virtual List<Order>? Orders { get; set; }

    /// <summary>
    /// Количество заказов клиента
    /// </summary>
    public int OrderCount => Orders?.Count ?? 0;

    /// <summary>
    /// Метод для расчета общего количества доставленных товаров
    /// </summary>
    /// <returns>Общее количество доставок</returns>
    public int GetTotalDeliveries()
    {
        return Orders?.Count(o => o.Status == OrderStatus.Completed) ?? 0;
    }

    /// <summary>
    /// Возвращает полное имя клиента
    /// </summary>
    /// <returns>ФИО клиента</returns>
    public override string ToString()
    {
        return string.IsNullOrEmpty(Patronymic)
            ? $"{LastName} {FirstName}"
            : $"{LastName} {FirstName} {Patronymic}";
    }
}