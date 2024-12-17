using OpenTK.Input;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameLibrary;
using GameLibrary.Tank;

namespace TankBattle.Managers
{
    public class PlayerManager : IGameObjectManager
    {
        private KeyboardState _keyboardState;
        private NetworkManager _networkManager;
        private GameManager _gameManager;

        private const int PlayerTickInterval = 50;

        private List<Key> CurrentPlayerInput = new List<Key> { Key.W, Key.S, Key.A, Key.D };
        private List<Key> CurrentPlayerFire = new List<Key> { Key.Z, Key.M };
        private int _currentPlayerTicks = PlayerTickInterval;

        public void SetManagers(NetworkManager networkManager, GameManager gameManager)
        {
            _networkManager = networkManager ?? throw new ArgumentNullException(nameof(networkManager));
            _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
        }

        public void PlayerControl(int textureIdLeft, int textureIdRight, int textureIdUp, int textureIdDown)
        {
            _keyboardState = Keyboard.GetState();
            var moveVector = Vector2.Zero;

            var playArea = ColliderManager.screenBorderCollider;
            var playerCollider = _networkManager.CurrentPlayer.GetCollider();

            if (_keyboardState.IsKeyDown(CurrentPlayerInput[0]) && playerCollider.Y < playArea.Width - playArea.Y - 0.02f)
            {
                SetPlayerDirection(textureIdUp, 0, ref moveVector, Vector2.UnitY * -0.001f);
            }
            else if (_keyboardState.IsKeyDown(CurrentPlayerInput[1]) && playerCollider.Y > 0)
            {
                SetPlayerDirection(textureIdDown, 1, ref moveVector, Vector2.UnitY * 0.001f);
            }
            else if (_keyboardState.IsKeyDown(CurrentPlayerInput[2]) && playerCollider.X > playArea.X)
            {
                SetPlayerDirection(textureIdLeft, 2, ref moveVector, Vector2.UnitX * -0.001f);
            }
            else if (_keyboardState.IsKeyDown(CurrentPlayerInput[3]) && playerCollider.X < playArea.Width - 0.1f)
            {
                SetPlayerDirection(textureIdRight, 3, ref moveVector, Vector2.UnitX * 0.001f);
            }

            if (moveVector != Vector2.Zero)
            {
                moveVector = Vector2.Normalize(moveVector) * _networkManager.CurrentPlayer.Speed;
                _networkManager.CurrentPlayer.Move(moveVector);

                if (ColliderManager.CollisionPlayerWithWalls(_networkManager.CurrentPlayer))
                {
                    _networkManager.CurrentPlayer.Move(-moveVector);
                }
            }
        }

        private void SetPlayerDirection(int textureId, int direction, ref Vector2 moveVector, Vector2 movementDirection)
        {
            _networkManager.CurrentPlayer.TankID = textureId;
            _networkManager.CurrentPlayer.Direction = direction;
            moveVector = movementDirection;
        }

        public void PlayerShoot()
        {
            _currentPlayerTicks++;
            _keyboardState = Keyboard.GetState();

            bool isFireKeyPressed = _keyboardState.IsKeyDown(CurrentPlayerFire[0]);

            if (isFireKeyPressed && _currentPlayerTicks >= PlayerTickInterval)
            {
                _currentPlayerTicks = 0;
                if (_networkManager.CurrentPlayer.Ammo > 0)
                {
                    FireBullet();
                }
            }
        }

        private void FireBullet()
        {
            Bullet bullet = new CommonBullet(
                _networkManager.CurrentPlayer.GetGunPosition(),
                TextureManager.commonBulletLeftTexture,
                TextureManager.commonBulletRightTexture,
                TextureManager.commonBulletUpTexture,
                TextureManager.commonBulletDownTexture,
                _networkManager.CurrentPlayer.Direction
            );

            if (_networkManager.CurrentPlayer == _gameManager.FirstPlayer)
            {
                _networkManager._firstPlayerBulletList.Add(bullet);
            }
            else
            {
                _networkManager._secondPlayerBulletList.Add(bullet);
            }

            _networkManager.BulletData = new BulletData
            {
                ShooterID = _networkManager.CurrentPlayer.TankID,
                PositionX = bullet.PositionCenter.X,
                PositionY = bullet.PositionCenter.Y,
                Direction = _networkManager.CurrentPlayer.Direction,
                BulletType = SerializeAmmo(bullet)
            };

            _networkManager.CurrentPlayer.Ammo--;
        }

        private static int SerializeAmmo(Bullet bullet)
        {
            if (bullet is CommonBullet)
            {
                return 0;
            }

            return -1;
        }

        public void CheckPlayerDamage(List<Bullet> bulletList,  AbstractTank player)
        {
            for (int i = bulletList.Count - 1; i >= 0; i--)
            {
                bulletList[i].Fire();

                if (player.GetCollider().IntersectsWith(bulletList[i].GetCollider()))
                {
                    player.GetDamage(bulletList[i].Damage);
                    bulletList.RemoveAt(i);
                    continue;
                }

                if (!bulletList[i].GetCollider().IntersectsWith(ColliderManager.bulletRangeCollider))
                {
                    bulletList.RemoveAt(i);
                    break;
                }

                if (ColliderManager.wallColliders.Any(wall => bulletList[i].GetCollider().IntersectsWith(wall)))
                {
                    bulletList.RemoveAt(i);
                    break;
                }
            }
        }

        public void UpdatePlayerTexture()
        {
            UpdateTexture(_networkManager.CurrentPlayer);
            UpdateTexture(_networkManager.NetworkPlayer);
        }

        private void UpdateTexture(AbstractTank player)
        {
            int texture = player == _gameManager.FirstPlayer ? GetFirstPlayerTexture(player) : GetSecondPlayerTexture(player);
            player.TankID = texture;
        }

        private int GetFirstPlayerTexture(AbstractTank player)
        {
            switch (player.Direction)
            {
                case 0:
                    return TextureManager.firstTankTextureUP;
                case 1:
                    return TextureManager.firstTankTextureDown;
                case 2:
                    return TextureManager.firstTankTextureLeft;
                case 3:
                    return TextureManager.firstTankTextureRight;
                default:
                    throw new InvalidOperationException("Invalid direction for first player");
            }
        }

        private int GetSecondPlayerTexture(AbstractTank player)
        {
            switch (player.Direction)
            {
                case 0:
                    return TextureManager.secondTankTextureUP;
                case 1:
                    return TextureManager.secondTankTextureDown;
                case 2:
                    return TextureManager.secondTankTextureLeft;
                case 3:
                    return TextureManager.secondTankTextureRight;
                default:
                    throw new InvalidOperationException("Invalid direction for second player");
            }
        }


        public void SetControlSchemeToWASD()
        {
            SetControlScheme(new[] { Key.W, Key.S, Key.A, Key.D }, new[] { Key.Z });
        }

        public void SetControlSchemeToArrowKeys()
        {
            SetControlScheme(new[] { Key.Up, Key.Down, Key.Left, Key.Right }, new[] { Key.M });
        }

        private void SetControlScheme(Key[] movementKeys, Key[] fireKeys)
        {
            CurrentPlayerInput.Clear();
            CurrentPlayerFire.Clear();

            CurrentPlayerInput.AddRange(movementKeys);
            CurrentPlayerFire.AddRange(fireKeys);
        }
    }
}
