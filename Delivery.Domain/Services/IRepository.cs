namespace Delivery.Domain.Services;

/// <summary>
/// Обобщенный интерфейс для операций над коллекциями доменных сущностей
/// </summary>
/// <typeparam name="TEntity">Тип доменной сущности</typeparam>
/// <typeparam name="TKey">Тип ключевого поля сущности</typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    /// <summary>
    /// Получение всего списка сущностей
    /// </summary>
    /// <returns>Список всех сущностей</returns>
    Task<IList<TEntity>> GetAll();
    /// <summary>
    /// Получение сущности по ее идентификатору
    /// </summary>
    /// <param name="key">Идентификатор</param>
    /// <returns>Сущность</returns>
    Task<TEntity?> Get(TKey key);
    /// <summary>
    /// Добавление новой сущности в коллекцию
    /// </summary>
    /// <param name="entity">Новая сущность</param>
    /// <returns>Результат операции</returns>
    Task<TEntity> Add(TEntity entity);
    /// <summary>
    /// Обновление полей существующей сущности
    /// </summary>
    /// <param name="entity">Отредактированная сущность</param>
    /// <returns>Результат операции</returns>
    Task<TEntity> Update(TEntity entity);
    /// <summary>
    /// Удаление сущности из коллекции
    /// </summary>
    /// <param name="key">Идентификатор удаляемой сущности</param>
    /// <returns>Результат операции</returns>
    Task<bool> Delete(TKey key);
}
