using GameLibrary.Tank;
using GameLibrary.TankDecorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;

namespace GameTests
{
    [TestClass]
    public class DamageTest
    {
        private AbstractTank CreateTankWithHealthAndArmor(int health, int armor)
        {
            AbstractTank tank = new BasicTank(Vector2.Zero, 0)
            {
                Health = health,
                Armor = armor
            };
            return tank;
        }

        [TestMethod]
        public void GetDamageTestMethod()
        {
            // Arrange
            AbstractTank tank = CreateTankWithHealthAndArmor(100, 0);
            int expectedHealth = 60;

            // Act
            tank.GetDamage(40);
            int actualHealth = tank.Health;

            // Assert
            Assert.AreEqual(expectedHealth, actualHealth);
        }

        [TestMethod]
        public void GetDamageWithArmorTestMethod()
        {
            // Arrange
            AbstractTank tank = CreateTankWithHealthAndArmor(100, 25);
            int expectedHealth = 85;

            // Act
            tank.GetDamage(40);
            int actualHealth = tank.Health;

            // Assert
            Assert.AreEqual(expectedHealth, actualHealth);
        }

        [TestMethod]
        public void GetDamageWithArmorAndBoostTestMethod()
        {
            // Arrange
            Random random = new Random();
            AbstractTank tank = CreateTankWithHealthAndArmor(100, 25);
            int boostHealth = random.Next(1, 16);
            int expectedHealth = 85 + boostHealth;

            // Act
            tank.GetDamage(40);
            tank = new HealthBoostDecorator(tank, boostHealth);
            int actualHealth = tank.Health;

            // Assert
            Assert.AreEqual(expectedHealth, actualHealth);
        }
    }
}
