namespace Common.PersistentManager
{
	/// <summary>
	/// Интерфейс сохраняемого объекта.
	/// </summary>
	/// <typeparam name="T">Класс сохраняемого объекта.</typeparam>
	public interface IPersistent<T> where T : new()
	{
		/// <summary>
		/// Уникальный идентификатор сохраняемого объекта.
		/// </summary>
		string PersistentId { get; }

		/// <summary>
		/// Метод восстановления объекта из данных.
		/// </summary>
		/// <param name="data">Данные, из которых должен восстанавливаться объект.</param>
		/// <typeparam name="T1">Тип данных, из которых восстанавливается объект.</typeparam>
		void Restore<T1>(T1 data) where T1 : IPersistent<T>;
	}
}