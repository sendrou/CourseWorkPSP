using System;
using System.Threading.Tasks;
namespace TcpConnectionLibrary
{
    public interface ITcpHandler
    {
        Task UpdateData<T>(T data); // Метод для отправки и получения данных
        event Action<object> OnGetData; // Событие при получении данных
        void ClearAllListeners(); // Метод для очистки всех подписок на событие
    }

}