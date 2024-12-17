using GameLibrary;
using GameLibrary;
using GameLibrary.Tank;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle.Managers
{

    public static class ColliderManager
    {
        public static RectangleF screenBorderCollider { get; private set; }
        public static RectangleF bulletRangeCollider { get; private set; }
        public static List<RectangleF> wallColliders { get; private set; }


        public static void SetupColliders()
        {
            // Устанавливаем коллайдеры для границ экрана и области выстрела
            screenBorderCollider = new RectangleF(0f, 0.125f, 1.025f, 0.875f);
            bulletRangeCollider = new RectangleF(-0.5f, -0.5f, 1.5f, 1.5f);

            wallColliders = new List<RectangleF>();

            // Параметры карты
            Vector2 mapMin = new Vector2(-1.0f, -1.0f); // Нижний левый угол
            Vector2 mapMax = new Vector2(1.0f, 1.0f);   // Верхний правый угол

            // Параметры кирпичей
            float brickWidth = 0.08f; // Ширина одного кирпича
            float brickHeight = 0.1f; // Высота одного кирпича

            // Размеры карты
            int mapWidth = 25; // Ширина карты в блоках
            int mapHeight = 20; // Высота карты в блоках

            // Данные карты (1 - есть кирпич, 0 - нет)

            int[] mapData = new int[] {
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
            CreateMapColliders( mapHeight,  mapWidth,  mapData,  mapMin,  mapMax,  brickWidth,  brickHeight);
        }
        

        
        // Метод для создания коллайдеров блоков карты
        private static void CreateMapColliders(int mapHeight, int mapWidth, int[] mapData, Vector2 mapMin, Vector2 mapMax, float brickWidth, float brickHeight)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int index = y * mapWidth + x;

                    // Если на текущей позиции есть блок (значение 1)
                    if (mapData[index] == 1)
                    {
                        // Рассчитываем координаты блока
                        float posX = mapMin.X + (x / (float)mapWidth) * (mapMax.X - mapMin.X);
                        float posY = mapMin.Y + ((mapHeight - 1 - y) / (float)mapHeight) * (mapMax.Y - mapMin.Y);

                        // Нормализуем координаты в диапазон [0, 1]
                        float normalizedPosX = (posX + 1.0f) * 0.5f;
                        float normalizedPosY = (posY + 1.0f) * 0.5f;

                        // Нормализуем размеры блока
                        float normalizedBrickWidth = brickWidth * 0.5f;
                        float normalizedBrickHeight = brickHeight * 0.5f;

                        // Создаем коллайдер блока
                        RectangleF blockCollider = new RectangleF(normalizedPosX, normalizedPosY, normalizedBrickWidth, normalizedBrickHeight);
                        wallColliders.Add(blockCollider);
                    }
                }
            }
        }

        // Метод для проверки столкновения игрока с блоками
        public static bool CollisionPlayerWithWalls(AbstractTank tank)
        {
            // Получаем координаты и размеры танка
            float tankX = tank.GetCollider().X - tank.GetCollider().Width / 2;
            float tankY = tank.GetCollider().Y - tank.GetCollider().Height / 2;
            float tankWidth = tank.GetCollider().Width;
            float tankHeight = tank.GetCollider().Height;

            // Проверяем столкновение с каждым блоком
            foreach (RectangleF wall in wallColliders)
            {
                // Рассчитываем коллайдер блока с небольшим отступом
                float wallX = wall.X - wall.Width + 0.01f;
                float wallY = wall.Y - wall.Height - 0.01f;
                float wallWidth = wall.Width + 0.02f;
                float wallHeight = wall.Height - 0.02f;

                // Проверяем столкновение между танком и блоком
                if (CollisionRect(tankX, tankY, wallX, wallY, tankWidth, tankHeight, wallWidth, wallHeight))
                {
                    return true; // Столкновение произошло
                }
            }

            return false; // Столкновений нет
        }

        public static bool CollisionRect(float r1x, float r1y, float r2x, float r2y, float r1w, float r1h, float r2w, float r2h)
        {
            // Проверка пересечения двух прямоугольников
            return r1x + r1w >= r2x &&
                   r1x <= r2x + r2w &&
                   r1y + r1h >= r2y &&
                   r1y <= r2y + r2h;
        }
    }
}
