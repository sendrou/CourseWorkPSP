using GameLibrary.Tank;
using System;

namespace GameLibrary.TankDecorators
{
    public class HealthBoostDecorator : TankDecorator
    {
        private int _extraHealth;
        private const int MaxHealth = 200;

        public HealthBoostDecorator(AbstractTank tank, int extraHealth) : base(tank)
        {
            _extraHealth = extraHealth;
            _tank.Health = CalculateHealth(_tank.Health, _extraHealth);
        }

        private int CalculateHealth(int currentHealth, int extraHealth)
        {
            if (currentHealth >= MaxHealth)
            {
                return MaxHealth;
            }

            return Math.Min(currentHealth + extraHealth, MaxHealth);
        }

        public override int Health
        {
            get => Math.Min(_tank.Health + _extraHealth, MaxHealth);
            set => _tank.Health = value;
        }
    }
}
