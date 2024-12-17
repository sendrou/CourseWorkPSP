using OpenTK;
using OpenTK.Graphics.OpenGL;
using GameLibrary;
using PrizesLibrary.Prizes;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace TankBattle.Managers
{
    public class RenderManager
    {
        private readonly GameManager _gameManager;
        private readonly NetworkManager _networkManager;

        private bool _startBackground = true;

        // Map parameters
        private readonly Vector2 _mapMin = new Vector2(-1.0f, -1.0f); // Bottom-left corner
        private readonly Vector2 _mapMax = new Vector2(1.0f, 1.0f);   // Top-right corner

        // Brick parameters
        private const float BrickWidth = 0.08f;
        private const float BrickHeight = 0.1f;

        // Map size
        private const int MapWidth = 25;
        private const int MapHeight = 20;

        // Map data 
        private readonly int[] _mapData = new int[] {
            0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
            1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1,
            0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0
    };

    public RenderManager(GameManager gameManager, NetworkManager networkManager)
        {
            _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
            _networkManager = networkManager ?? throw new ArgumentNullException(nameof(networkManager));
        }

        public void GlControl_Render(TimeSpan elapsedTime)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RenderBackground();

            if (!_startBackground)
            {
                RenderMap();
                _gameManager.FirstPlayer.Render();
                _gameManager.SecondPlayer.Render();
            }

            // Render bullets and prizes
            RenderBullets();
            RenderPrizes();
        }

        public void UpdateBackground()
        {
            _startBackground = false;
        }

        private void RenderBackground()
        {
            var backgroundTexture = _startBackground ? TextureManager.backGroundTexture : TextureManager.mapBackGroundTexture;
            var vertices = new Vector2[4]
            {
                new Vector2(-1.0f, -1.0f),
                new Vector2(1.0f, -1.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(-1.0f, 1.0f)
            };
            ObjectRender.RenderObjects(backgroundTexture, vertices);
        }

        private void RenderMap()
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    int index = y * MapWidth + x;
                    if (_mapData[index] == 1)
                    {
                        RenderBrick(x, y);
                    }
                }
            }
        }

        private void RenderBrick(int x, int y)
        {
            float posX = _mapMin.X + (x / (float)MapWidth) * (_mapMax.X - _mapMin.X);
            float posY = _mapMin.Y + (y / (float)MapHeight) * (_mapMax.Y - _mapMin.Y);

            Vector2[] blockVertices = new Vector2[4]
            {
                new Vector2(posX, posY), // Bottom-left corner
                new Vector2(posX + BrickWidth, posY), // Bottom-right corner
                new Vector2(posX + BrickWidth, posY + BrickHeight), // Top-right corner
                new Vector2(posX, posY + BrickHeight) // Top-left corner
            };

            ObjectRender.RenderObjects(TextureManager.brickCenterTexture, blockVertices);
        }

        private void RenderBullets()
        {
            foreach (Bullet bullet in _networkManager._firstPlayerBulletList)
            {
                bullet.Render();
            }
            foreach (Bullet bullet in _networkManager._secondPlayerBulletList)
            {
                bullet.Render();
            }
        }

        private void RenderPrizes()
        {
            foreach (Prize prize in _gameManager.PrizeList)
            {
                prize.Render();
            }
        }
    }
}
