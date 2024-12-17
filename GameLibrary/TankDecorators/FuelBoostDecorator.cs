using GameLibrary.Tank;
using OpenTK;
using System;

namespace GameLibrary.TankDecorators
{
    public class FuelBoostDecorator : TankDecorator
    {
        private int _extraFuel;
        private const int MaxFuel = 3000;

        public FuelBoostDecorator(AbstractTank tank, int extraFuel) : base(tank)
        {
            _extraFuel = extraFuel;
            _tank.Fuel = CalculateFuel(_tank.Fuel, _extraFuel);
        }

        private int CalculateFuel(int currentFuel, int extraFuel)
        {
            if (currentFuel >= MaxFuel)
            {
                return MaxFuel;
            }

            return Math.Min(currentFuel + extraFuel, MaxFuel);
        }

        public override int Fuel
        {
            get => _tank.Fuel;
            set => _tank.Fuel = value;
        }

        public override void Move(Vector2 movement)
        {
            if (Fuel <= 0) return;

            if (_extraFuel > 0)
            {
                _extraFuel--;
            }
            else
            {
                _tank.Move(movement);
            }
        }
    }
}
