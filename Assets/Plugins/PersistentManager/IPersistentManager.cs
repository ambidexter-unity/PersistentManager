using Common.Service;

namespace Common.PersistentManager
{
	/// <summary>
	/// Менеджер сохранения состояния игровых объектов.
	/// </summary>
	public interface IPersistentManager : IGameService
	{
		/// <summary>
		/// Сохранить состояние объекта.
		/// </summary>
		/// <param name="data">Сохраняемый объект.</param>
		/// <param name="asPlayerPrefs">Флаг, указывающий сохранять объект как PlayerPrefs.</param>
		/// <param name="lazy">Флаг, указывающий сохранять данные на диск не сразу,
		/// а в момент деактивации приложения</param>
		/// <typeparam name="T">Тип сохраняемого объекта.</typeparam>
		void Persist<T>(T data, bool asPlayerPrefs = false, bool lazy = false) where T : IPersistent<T>, new();

		/// <summary>
		/// Восстановить состояние объекта.
		/// </summary>
		/// <param name="data">Объект, подлежащий восстановлению.</param>
		/// <typeparam name="T">Тип восстанавливаемого объекта.</typeparam>
		/// <returns>Возвращает <code>true</code>, если данные были найдены и объект восстановлен.</returns>
		bool Restore<T>(T data) where T : IPersistent<T>, new();

		/// <summary>
		/// Получить сохраненное состояние объекта указанного типа.
		/// </summary>
		/// <typeparam name="T">Тип объекта, состояние которого запрашивается.</typeparam>
		/// <returns>Экземпляр объекта с восстановленным состоянием.</returns>
		T GetPersistentValue<T>() where T : IPersistent<T>, new();
		
		/// <summary>
		/// Удалить сохраненные данные для указанного объекта.
		/// </summary>
		/// <param name="data">Объект, данные которого удаляются.</param>
		/// <typeparam name="T">Тип объекта, данные которого удаляются.</typeparam>
		/// <returns>Возвращает <code>true</code>, если данные были найдены и успешно удалены.</returns>
		bool Remove<T>(T data) where T : IPersistent<T>, new();

		/// <summary>
		/// Удаляет сохраненные данные для объекта с указанным идентификатором.
		/// </summary>
		/// <param name="id">Идентификатор объекта, данные которого удаляются.</param>
		/// <returns>Возвращает <code>true</code>, если данные были найдены и успешно удалены.</returns>
		bool Remove(string id);
	}
}