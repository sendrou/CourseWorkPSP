using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnectionLibrary
{
    public class Server : ITcpHandler, IDisposable
    {
        public event Action<object> OnGetData;

        public Socket ServerSocket { get; private set; }
        private Socket _clientSocket;
        private int _port;

        public Server(int port = 8000)
        {
            _port = port;
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipAddress = new IPEndPoint(IPAddress.Any, _port);
            ServerSocket.Bind(ipAddress);



            ServerSocket.Listen(1);
        }

        public async Task Start()
        {
            Console.WriteLine("Waiting for a connection...");
            _clientSocket = await Task.Run(() => ServerSocket.Accept());

            // Получение локального IP-адреса и порта
            var localEndPoint = _clientSocket.LocalEndPoint as IPEndPoint;
            var remoteEndPoint = _clientSocket.RemoteEndPoint as IPEndPoint;

            Console.WriteLine("Client connected");
            if (localEndPoint != null)
            {
                Console.WriteLine($"Server is bound to IP: {localEndPoint.Address}, Port: {localEndPoint.Port}");
            }
            if (remoteEndPoint != null)
            {
                Console.WriteLine($"Client connected from IP: {remoteEndPoint.Address}, Port: {remoteEndPoint.Port}");
            }
        }

        private async Task<Socket> AcceptClientAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return ServerSocket.Accept();
                }
                catch (Exception ex)
                {
                    LogError($"Error while accepting client: {ex.Message}");
                    return null;
                }
            });
        }

        public async Task UpdateData<T>(T obj)
        {
            try
            {
                // Читаем данные от клиента асинхронно
                var requestText = await ReceiveDataFromClientAsync();
                Console.WriteLine("REQUEST TEXT: " + requestText);

                // Проверяем JSON перед десериализацией
                if (string.IsNullOrWhiteSpace(requestText))
                {
                    Console.WriteLine("Received empty or null data");
                    return;
                }

                var request = JsonConvert.DeserializeObject<T>(requestText);
                Console.WriteLine("REQUEST: " + request);

                // Отправляем ответ клиенту
                var dataText = JsonConvert.SerializeObject(obj);
                byte[] data = Encoding.UTF8.GetBytes(dataText);
                _clientSocket.Send(data);

                OnGetData?.Invoke(request);
            }
            catch (JsonException jsonEx)
            {
                LogError($"JSON error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                LogError($"General error: {ex.Message}");
            }
        }

        private async Task<string> ReceiveDataFromClientAsync()
        {
            var buffer = new byte[256];
            var data = new List<byte>();

            try
            {
                while (true)
                {
                    int bytesRead = await Task.Run(() => _clientSocket.Receive(buffer));
                    if (bytesRead == 0)
                        break;

                    // Добавляем полученные данные в список байтов
                    data.AddRange(buffer.Take(bytesRead));

                    // Если меньше данных, чем размер буфера, завершаем чтение
                    if (bytesRead < buffer.Length)
                        break;
                }
            }
            catch (Exception ex)
            {
                LogError($"Error while receiving data: {ex.Message}");
            }

            return Encoding.UTF8.GetString(data.ToArray());
        }

        private void LogError(string message)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + message);
            Console.ResetColor();
        }

        public void Dispose()
        {
            _clientSocket?.Close();
            ServerSocket.Close();
            ServerSocket.Dispose();
        }

        public void ClearAllListeners()
        {
            OnGetData = null;
        }
    }
}
