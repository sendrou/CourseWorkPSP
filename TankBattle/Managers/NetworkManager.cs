using GameLibrary;
using GameLibrary.Tank;
using OpenTK;
using PrizesLibrary.Factories;
using PrizesLibrary.Prizes;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TcpConnectionLibrary;

namespace TankBattle.Managers
{
    public class NetworkManager
    {
        public List<Bullet> _firstPlayerBulletList = new List<Bullet>();
        public List<Bullet> _secondPlayerBulletList = new List<Bullet>();

        public AbstractTank CurrentPlayer { get; set; }
        public AbstractTank NetworkPlayer { get; set; }
        public Prize CurrentPrize { get; set; }
        public List<Prize> CurrentPrizeList { get; set; }

        private ITcpHandler _networkConnection;
        private NetworkData _currentNetworkData = new NetworkData();
        public BulletData BulletData;

        public PrizeFactory PrizeFactory { get; set; }

        private GameManager _gameManager;
        private UIManager _uiManager;
        private TimeManager _timeManager;
        private PlayerManager _playerManager;
        private RenderManager _renderManager;

        private Random _random;
        public Client Client { get; private set; }
        public Server Server { get; private set; }
        public NetworkManager(GameManager gameManager, UIManager uiManager, TimeManager timeManager, PlayerManager playerManager)
        {
            _gameManager = gameManager;
            _uiManager = uiManager;
            _timeManager = timeManager;
            _playerManager = playerManager;
            _uiManager.SetGameManager(_gameManager);
        }

        public void SetNetworkStartData(ITcpHandler networkConnection, bool isLeftPlayer, int seed)
        {
            _networkConnection = networkConnection;

            CurrentPlayer = isLeftPlayer ? _gameManager.FirstPlayer : _gameManager.SecondPlayer;
            NetworkPlayer = isLeftPlayer ? _gameManager.SecondPlayer : _gameManager.FirstPlayer;

            CurrentPrizeList = _gameManager.PrizeList;
            _random = new Random(seed);
            PrizeFactory = new PrizeFactory(_random);

            _networkConnection.OnGetData += OnGetData;
        }

        private void OnGetData(object obj)
        {
            try
            {
                var networkData = (NetworkData)obj;
                UpdateNetworkPlayerData(networkData);
                UpdateBulletsData(networkData.BulletData);
            }
            catch (Exception ex)
            {
                LogError("Error in OnGetNetworkData", ex);
            }
        }

        private void UpdateNetworkPlayerData(NetworkData networkData)
        {
            // Обновляем данные для сетевого игрока
            NetworkPlayer.PositionCenter = new Vector2(networkData.PositionX, networkData.PositionY);
            NetworkPlayer.Health = networkData.Health;
            NetworkPlayer.Armor = networkData.Armor;
            NetworkPlayer.Fuel = networkData.Fuel;
            NetworkPlayer.Ammo = networkData.Ammo;
            NetworkPlayer.Speed = networkData.Speed;
            NetworkPlayer.Direction = networkData.Direction;
            // Обновляем текстуру сетевого игрока
            _playerManager.UpdatePlayerTexture();
        }

        private void UpdateBulletsData(BulletData bulletData)
        {
            if (bulletData == null) return;

            // Создание и добавление пули
            if (bulletData.ShooterID != CurrentPlayer.TankID)
            {
                if (CurrentPlayer == _gameManager.FirstPlayer)
                {
                    _secondPlayerBulletList.Add(_gameManager.CreateNewAmmo(bulletData));
                }
                else
                {
                    _firstPlayerBulletList.Add(_gameManager.CreateNewAmmo(bulletData));
                }
            }
        }

        public async Task UpdateNetworkData()
        {
            try
            {
                _currentNetworkData.PositionX = CurrentPlayer.PositionCenter.X;
                _currentNetworkData.PositionY = CurrentPlayer.PositionCenter.Y;
                _currentNetworkData.BulletData = BulletData;
                _currentNetworkData.Health = CurrentPlayer.Health;
                _currentNetworkData.Armor = CurrentPlayer.Armor;
                _currentNetworkData.Fuel = CurrentPlayer.Fuel;
                _currentNetworkData.Ammo = CurrentPlayer.Ammo;
                _currentNetworkData.Speed = CurrentPlayer.Speed;
                _currentNetworkData.Direction = CurrentPlayer.Direction;
                await _networkConnection.UpdateData(_currentNetworkData);
                BulletData = null;
            }
            catch (Exception ex)
            {
                LogError("Error in UpdateNetworkData", ex);
            }
        }

        public async Task StartServer(RenderManager renderManager)
        {
            try
            {
                _renderManager = renderManager;
                Server = new Server();
                int seed = new Random().Next();

                Server.OnGetData += (_) => StartGame(seed, Server, true);

                await Server.Start();
                Console.WriteLine("Server started. Client connected.");
                await Server.UpdateData<int>(seed);
            }
            catch (Exception ex)
            {
                LogError("Error in StartServer", ex);
            }
        }

        public async Task StartClient(RenderManager renderManager, TextBox ipAddressInput)
        {
            try
            {
                _renderManager = renderManager;
                Client = new Client(ipAddressInput.Text);

                Client.OnGetData += (obj) =>
                {
                    StartGame((int)obj, Client, false);
                };

                await Client.Connect();
                Console.WriteLine("Client connected successfully.");
                await Client.GetData<int>();
            }
            catch (Exception ex)
            {
                LogError("Error in StartClient", ex);
            }
        }

        private void StartGame(int seed, ITcpHandler networkConnection, bool isServer)
        {
            try
            {
                _renderManager.UpdateBackground();
                networkConnection.ClearAllListeners();

                SetNetworkStartData(networkConnection, isServer, seed);

                Application.Current?.Dispatcher.Invoke(() =>
                {
                    _uiManager.HideClientServerSelection();
                    _timeManager.LaunchTimers(this);
                });
            }
            catch (Exception ex)
            {
                LogError("Error in StartGame", ex);
            }
        }

        private void LogError(string message, Exception ex)
        {
            Console.WriteLine($"{message}: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}
