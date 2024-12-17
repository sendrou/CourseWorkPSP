using OpenTK;
using PrizesLibrary.Factories;
using PrizesLibrary.Prizes;
using System;
using System.Windows.Threading;

namespace TankBattle.Managers
{
    public class TimeManager
    {
        private DispatcherTimer _gameTimer;
        private DispatcherTimer _prizeTimer;

        private readonly GameManager _gameManager;
        private readonly PrizeManager _prizeManager;

        private readonly Random _random;
        private readonly PrizeFactory _prizeFactory;

        public TimeManager(GameManager gameManager, PrizeManager prizeManager)
        {
            _random = new Random();
            _prizeFactory = new PrizeFactory(_random);

            _gameManager = gameManager;
            _prizeManager = prizeManager;
        }

        public void LaunchTimers(NetworkManager networkManager)
        {
            Console.WriteLine("Initializing timers...");

            InitializeGameTimer(networkManager);
            InitializePrizeTimer();

            StartTimers();
        }

        private void InitializeGameTimer(NetworkManager networkManager)
        {
            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            _gameTimer.Tick += (sender, e) => _gameManager.GameTimer_Tick(networkManager, sender, e);
            Console.WriteLine("Game timer initialized");
        }

        private void InitializePrizeTimer()
        {
            _prizeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            _prizeTimer.Tick += _prizeManager.PrizeTimer_Tick;
            Console.WriteLine("Prize timer initialized");
        }

        private void StartTimers()
        {
            _gameTimer.Start();
            Console.WriteLine("Game timer started");

            _prizeTimer.Start();
            Console.WriteLine("Prize timer started");
        }
    }
}
