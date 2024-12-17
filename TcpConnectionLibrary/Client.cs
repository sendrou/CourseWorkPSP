using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnectionLibrary
{
    public class Client : ITcpHandler, IDisposable
    {
        public event Action<object> OnGetData;

        public Socket ClientSocket { get; private set; }
        private string _address;
        private int _port;

        public Client(string ipAddress, int port = 8000)
        {
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _address = ipAddress;
            _port = port;
            Console.WriteLine("IP Address: " + _address);
        }

        public async Task Connect()
        {
            try
            {
                await Task.Run(() =>
                {
                    ClientSocket.Connect(_address, _port);
                    Console.WriteLine("Connected to server");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Connect: {ex.Message}");
            }
        }

        private async Task SendRequestAsync<T>(T requestObj)
        {
            try
            {
                var requestJson = JsonConvert.SerializeObject(requestObj);
                var requestData = Encoding.UTF8.GetBytes(requestJson);

                await Task.Run(() =>
                {
                    ClientSocket.Send(requestData);
                    Console.WriteLine($"Request sent: {requestJson}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending request: {ex.Message}");
            }
        }

        private async Task<T> ReceiveResponseAsync<T>()
        {
            byte[] buffer = new byte[256];
            int bytesReceived = await Task.Run(() => ClientSocket.Receive(buffer));

            if (bytesReceived == 0)
            {
                Console.WriteLine("No data received");
                return default;
            }

            var resultText = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
            return JsonConvert.DeserializeObject<T>(resultText);
        }

        public async Task GetData<T>()
        {
            try
            {
                Console.WriteLine("Start getting data");

                // Отправляем запрос
                await SendRequestAsync(default(T));

                // Получаем ответ
                var result = await ReceiveResponseAsync<T>();
                if (result != null)
                {
                    OnGetData?.Invoke(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetNetworkData: {ex.Message}");
            }
        }

        public async Task UpdateData<T>(T obj)
        {
            try
            {
                // Отправка данных серверу
                await SendRequestAsync(obj);

                // Получение ответа от сервера
                if (ClientSocket.Connected)
                {
                    var result = await ReceiveResponseAsync<T>();
                    if (result != null)
                    {
                        OnGetData?.Invoke(result);
                    }
                }
                else
                {
                    Dispose();
                    Console.WriteLine("Socket is not connected.");
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateNetworkData: {ex.Message}");
            }
        }

        public void Dispose()
        {
            ClientSocket.Close();
            ClientSocket.Dispose();
        }

        public void ClearAllListeners()
        {
            OnGetData = null;
        }
    }
}
