using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using GameLibrary.Tank;

namespace TankBattle.Managers
{
    public class UIManager
    {
        private Label _firstPlayerInfo;
        private Label _secondPlayerInfo;
        private Button _serverButton;
        private Button _clientButton;
        private TextBox _ipAddressInput;
        private ComboBox _comboBox;
        private Label _gameOverLabel;
        private GameManager _gameManager;

        public UIManager(Button serverButton, Button clientButton, TextBox ipAddressInput, Label gameOverLabel, Label firstPlayerInfo, Label secondPlayerInfo, ComboBox comboBox)
        {
            _serverButton = serverButton;
            _clientButton = clientButton;
            _ipAddressInput = ipAddressInput;
            _gameOverLabel = gameOverLabel;
            _firstPlayerInfo = firstPlayerInfo;
            _secondPlayerInfo = secondPlayerInfo;
            _comboBox = comboBox;

            // Initially hide the game over label
            _gameOverLabel.Visibility = Visibility.Hidden;
        }

        public void SetGameManager(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void DisplayLocalIPAddress(Label labelIp, TextBox textBoxIp)
        {
            string localIp = GetLocalIPAddress();
            labelIp.Content = $"IP Address: {localIp}";
            textBoxIp.Text = localIp;
        }

        public string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var address = host.AddressList.FirstOrDefault(addr => addr.AddressFamily == AddressFamily.InterNetwork);
                return address?.ToString() ?? "IP not found";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting local IP address: {ex.Message}");
                return "IP not found";
            }
        }

        public void GameStateCheck(MainWindow mainWindow)
        {
            if (_gameManager.FirstPlayer.Health <= 0)
            {
                EndGame("Победил первый игрок", mainWindow);
            }
            else if (_gameManager.SecondPlayer.Health <= 0)
            {
                EndGame("Победил второй игрок", mainWindow);
            }
        }

        private void EndGame(string resultMessage, MainWindow mainWindow)
        {
            _gameOverLabel.Visibility = Visibility.Visible;
            mainWindow.Close();
            MessageBox.Show(resultMessage);
        }

        public void UpdatePlayerStats()
        {
            _firstPlayerInfo.Content = FormatPlayerStats(_gameManager.FirstPlayer);
            _secondPlayerInfo.Content = FormatPlayerStats(_gameManager.SecondPlayer);
        }

        private string FormatPlayerStats(AbstractTank player)
        {
            return $"HP:{player.Health}/200\nArmor:{player.Armor}/100\n" +
                   $"Ammo:{player.Ammo}/30\nSpeed:{player.Speed * 10f:F1}x\n" +
                   $"Fuel:{player.Fuel}/3000\n";
        }

        public void HideClientServerSelection()
        {
            try
            {
                Console.WriteLine("Hiding Role Selection UI elements");

                // Check if UI elements are null before hiding
                if (_ipAddressInput != null && _serverButton != null && _clientButton != null)
                {
                    _ipAddressInput.Visibility = Visibility.Collapsed;
                    _serverButton.Visibility = Visibility.Collapsed;
                    _clientButton.Visibility = Visibility.Collapsed;
                    _gameOverLabel.Visibility = Visibility.Collapsed;
                    _comboBox.Visibility = Visibility.Collapsed;

                    Console.WriteLine("Role Selection UI elements are hidden");
                }
                else
                {
                    Console.WriteLine("UI elements are null, cannot hide");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HideRoleSelection: {ex.Message}");
            }
        }
    }
}
