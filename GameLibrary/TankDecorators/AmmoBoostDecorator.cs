using GameLibrary.Tank;
using System;

namespace GameLibrary.TankDecorators
{
    public class AmmoBoostDecorator : TankDecorator
    {
        private readonly int _extraAmmo;
        private const int MaxAmmo = 30;

        public AmmoBoostDecorator(AbstractTank tank, int extraAmmo) : base(tank)
        {
            _extraAmmo = extraAmmo;
            _tank.Ammo = CalculateAmmo(_tank.Ammo, _extraAmmo);
        }

        private int CalculateAmmo(int currentAmmo, int extraAmmo)
        {
            if (currentAmmo >= MaxAmmo)
            {
                return MaxAmmo;
            }

            return Math.Min(currentAmmo + extraAmmo, MaxAmmo);
        }

        public override int Ammo
        {
            get => _tank.Ammo;
            set => _tank.Ammo = value;
        }
    }
}