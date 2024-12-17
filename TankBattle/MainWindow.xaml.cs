using System;
using System.Windows;
using TankBattle.Managers;



namespace TankBattle
{
    public partial class MainWindow
    {
        private PlayerManager _playerManager;
        private PrizeManager _prizeManager;
        private GameManager _gameManager;
        private UIManager _uiManager;
        private RenderManager _renderManager;
        private NetworkManager _networkManager;
        private TimeManager _timeManager;


        public MainWindow()
        {
            InitializeComponent();

            _uiManager = new UIManager(ServerButton, ClientButton, IpAddressInput, GameOverLabel, firstPlayerInfo, secondPlayerInfo, ControlSchemeComboBox);
            _playerManager = new PlayerManager();
            _prizeManager = new PrizeManager();

            _gameManager = new GameManager(glControl, this, _uiManager, _playerManager, _prizeManager);
            _timeManager = new TimeManager(_gameManager, _prizeManager);

            _networkManager = new NetworkManager(_gameManager, _uiManager, _timeManager, _playerManager);
            _playerManager.SetManagers(_networkManager, _gameManager);
            _prizeManager.SetManagers(_networkManager, _gameManager);

            _renderManager = new RenderManager(_gameManager, _networkManager);

            _uiManager.DisplayLocalIPAddress(IpAddressLabel, IpAddressInput);

        }

        private void ServerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 _networkManager.StartServer(_renderManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting server: {ex.Message}");
            }
        }
        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _networkManager.StartClient(_renderManager,IpAddressInput);
                


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting as client: {ex.Message}");
            }
        }

        private void GlControl_Render(TimeSpan obj)
        {
            _renderManager.GlControl_Render(obj);
        }
        private void ControlSchemeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (_playerManager == null)
                return;
            string selectedScheme = (ControlSchemeComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();
            if (selectedScheme == "Arrow Keys")
            {
                _playerManager.SetControlSchemeToArrowKeys();
            }
            else
            {
                _playerManager.SetControlSchemeToWASD();
            }
        }
    }
}
