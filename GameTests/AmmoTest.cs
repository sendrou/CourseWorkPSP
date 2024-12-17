using GameLibrary;
using GameLibrary.Tank;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System.Collections.Generic;

namespace GameTests
{
    [TestClass]
    public class AmmoTest
    {
        private AbstractTank CreateTankWithAmmo(int ammo)
        {
            AbstractTank tank = new BasicTank(Vector2.Zero, 0)
            {
                Ammo = ammo
            };
            return tank;
        }

        private List<Bullet> CreateBulletsForTank(AbstractTank tank)
        {
            List<Bullet> bulletsList = new List<Bullet>();
            while (bulletsList.Count < tank.Ammo)
            {
                bulletsList.Add(new CommonBullet(Vector2.Zero, 0, 0, 0, 0, 0));
            }
            return bulletsList;
        }

        [TestMethod]
        public void AmmoTestMethod()
        {
            // Arrange
            AbstractTank tank = CreateTankWithAmmo(10);
            List<Bullet> bulletsList = CreateBulletsForTank(tank);

            int expectedBulletsCount = 10;

            // Act
            int actualBulletsCount = bulletsList.Count;

            // Assert
            Assert.AreEqual(expectedBulletsCount, actualBulletsCount);
        }

        [TestMethod]
        public void AmmoWithShootTestMethod()
        {
            // Arrange
            AbstractTank tank = CreateTankWithAmmo(10);
            List<Bullet> bulletsList = CreateBulletsForTank(tank);

            int expectedBulletsCount = 3;

            // Act
            for (int i = bulletsList.Count - 1; i >= 3; i--)
            {
                bulletsList[i].Fire();
                bulletsList.RemoveAt(i); // Remove bullet after firing, regardless of whether it hits the player or leaves the screen
            }

            int actualBulletsCount = bulletsList.Count;

            // Assert
            Assert.AreEqual(expectedBulletsCount, actualBulletsCount);
        }
    }
}
