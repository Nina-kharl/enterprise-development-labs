using System.ComponentModel.DataAnnotations;

namespace Delivery.Domain.Models;

/// <summary>
/// Товар в системе доставки
/// </summary>
public class Product
{
    /// <summary>
    /// Уникальный идентификатор товара
    /// </summary>
    [Key]
    public required int Id { get; set; }

    /// <summary>
    /// Название товара
    /// </summary>
    [Required(ErrorMessage = "Название обязательно")]
    public string Name { get; set; }

    /// <summary>
    /// Описание товара
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Вес товара (в кг)
    /// </summary>
    [Range(0.1, 1000, ErrorMessage = "Вес должен быть от 0.1 до 1000 кг")]
    public double Weight { get; set; }

    /// <summary>
    /// Габариты товара (ДxШxВ см)
    /// </summary>
    public string? Dimensions { get; set; }

    /// <summary>
    /// Артикул товара
    /// </summary>
    public string? SKU { get; set; }

    /// <summary>
    /// Тип товара
    /// </summary>
    public ProductType ProductType { get; set; }

    /// <summary>
    /// Перегрузка метода, возвращающего строковое представление объекта
    /// </summary>
    /// <returns>Название товара</returns>
    public override string ToString() => Name ?? "<Без названия>";
}