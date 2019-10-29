using System.Threading.Tasks;

namespace Common.PersistentManager
{
    /// <summary>
    /// Интерфейс объекта, который хранится на сервере и который можно только скачать
    /// </summary>
    /// <typeparam name="T">Класс сохраняемого объекта.</typeparam>
    public interface iDownloadable<T> where T : new()
    {
        string Address { get; }
        /// <summary>
        /// Метод скачивания объекта.
        /// </summary>
        /// <param name="data">Данные, из которых должен восстанавливаться объект.</param>
        /// <typeparam name="T1">Тип данных, из которых восстанавливается объект.</typeparam>
        /// <returns>Возвращает <code>true</code>, если данные были скачены и объект восстановлен.</returns>
        void Download<T1>(T1 data) where T1 : iDownloadable<T>;
    }
}