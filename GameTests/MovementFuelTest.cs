using GameLibrary.Tank;
using GameLibrary.TankDecorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;

namespace GameTests
{
    [TestClass]
    public class MovementFuelTest
    {
        private AbstractTank CreateTankWithFuel(int fuel)
        {
            AbstractTank tank = new BasicTank(Vector2.Zero, 0)
            {
                Fuel = fuel
            };
            return tank;
        }

        [TestMethod]
        public void MovementCheckTestMethod()
        {
            // Arrange
            AbstractTank tank = CreateTankWithFuel(2000);
            int expectedFuel = 1990;

            // Act
            for (int i = 0; i < 10; i++)
            {
                tank.Move(Vector2.Zero);
            }
            int actualFuel = tank.Fuel;

            // Assert
            Assert.AreEqual(expectedFuel, actualFuel);
        }

        [TestMethod]
        public void MovementCheckWithBoostTest()
        {
            // Arrange
            Random random = new Random();
            int boost = random.Next(1, 250);
            AbstractTank tank = CreateTankWithFuel(2000);
            int expectedFuel = 1500 + boost;

            // Act
            for (int i = 0; i < 500; i++)
            {
                tank.Move(Vector2.Zero);
            }
            tank = new FuelBoostDecorator(tank, boost);
            int actualFuel = tank.Fuel;

            // Assert
            Assert.AreEqual(expectedFuel, actualFuel);
        }
    }
}
