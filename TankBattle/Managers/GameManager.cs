using GameLibrary;
using GameLibrary.Tank;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
using PrizesLibrary.Prizes;
using System;
using System.Collections.Generic;

namespace TankBattle.Managers
{
    public class GameManager
    {
        public AbstractTank FirstPlayer { get; set; }
        public AbstractTank SecondPlayer { get; set; }
        public List<Prize> PrizeList { get; set; }

        private UIManager _uiManager;
        private PlayerManager _playerManager;
        private PrizeManager _prizeManager;
        private MainWindow _mainWindow;

        public GameManager(GLWpfControl glControl, MainWindow mainWindow, UIManager uiManager, PlayerManager playerManager, PrizeManager prizeManager)
        {
            _playerManager = playerManager;
            _prizeManager = prizeManager;
            _mainWindow = mainWindow;
            _uiManager = uiManager;

            GameSettings(glControl);
            TextureManager.SetupTexture();
            ColliderManager.SetupColliders();
            SetupGameObjects();
        }

        public async void GameTimer_Tick(NetworkManager networkManager, object sender, EventArgs e)
        {
            // Проверка состояния игры
            _uiManager.GameStateCheck(_mainWindow);

            // Обработка повреждений игроков
            _playerManager.CheckPlayerDamage(networkManager._firstPlayerBulletList,  SecondPlayer);
            _playerManager.CheckPlayerDamage(networkManager._secondPlayerBulletList,  FirstPlayer);

            // Применение призов
            _prizeManager.ApplyPrize(PrizeList, FirstPlayer);
            _prizeManager.ApplyPrize(PrizeList, SecondPlayer);

            // Обработка выстрела игрока
            _playerManager.PlayerShoot();
            _uiManager.UpdatePlayerStats();

            // Управление игроками и обновление текстуры
            if (networkManager.CurrentPlayer == FirstPlayer)
            {
                _playerManager.PlayerControl(
                    TextureManager.firstTankTextureLeft,
                    TextureManager.firstTankTextureRight,
                    TextureManager.firstTankTextureUP,
                    TextureManager.firstTankTextureDown);
            }
            else
            {
                _playerManager.PlayerControl(
                    TextureManager.secondTankTextureLeft,
                    TextureManager.secondTankTextureRight,
                    TextureManager.secondTankTextureUP,
                    TextureManager.secondTankTextureDown);
            }

            // Обновление сетевых данных
            await networkManager.UpdateNetworkData();
        }

        private void GameSettings(GLWpfControl glControl)
        {
            var settings = new GLWpfControlSettings { MajorVersion = 3, MinorVersion = 6 };
            glControl.Start(settings);
            glControl.InvalidateVisual();
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        private void SetupGameObjects()
        {
            FirstPlayer = new BasicTank(new Vector2(-0.6f, -0.4f), TextureManager.firstTankTextureRight);
            SecondPlayer = new BasicTank(new Vector2(0.5f, 0f), TextureManager.secondTankTextureLeft);
            PrizeList = new List<Prize>();
        }

        public Bullet CreateNewAmmo(BulletData bulletData)
        {
            // Создание пули с передачей соответствующих текстур в зависимости от направления
            Bullet bullet = new CommonBullet(
                new Vector2(bulletData.PositionX, bulletData.PositionY),
                TextureManager.commonBulletLeftTexture,
                TextureManager.commonBulletRightTexture,
                TextureManager.commonBulletUpTexture,
                TextureManager.commonBulletDownTexture,
                bulletData.Direction);

            Console.WriteLine($"Bullet created with damage: {bullet.Damage}");
            return bullet;
        }
    }
}
