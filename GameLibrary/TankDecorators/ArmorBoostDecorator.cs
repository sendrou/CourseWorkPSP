using GameLibrary.Tank;
using System;

namespace GameLibrary.TankDecorators
{
    public class ArmorBoostDecorator : TankDecorator
    {
        private int _extraArmor;
        private const int MaxArmor = 50;

        public ArmorBoostDecorator(AbstractTank tank, int extraArmor) : base(tank)
        {
            _extraArmor = extraArmor;
            _tank.Armor = CalculateArmor(_tank.Armor, _extraArmor);
        }

        private int CalculateArmor(int currentArmor, int extraArmor)
        {
            if (currentArmor >= MaxArmor)
            {
                return MaxArmor;
            }

            return Math.Min(currentArmor + extraArmor, MaxArmor);
        }

        public override int Armor
        {
            get => Math.Min(_tank.Armor + _extraArmor, MaxArmor);
            set => _tank.Armor = value;
        }

        public override void GetDamage(int damage)
        {
            if (_extraArmor > 0)
            {
                int remainingDamage = damage - _extraArmor;
                _extraArmor = Math.Max(_extraArmor - damage, 0);

                if (remainingDamage > 0)
                {
                    _tank.GetDamage(remainingDamage);
                }
            }
            else
            {
                _tank.GetDamage(damage);
            }
        }
    }
}
