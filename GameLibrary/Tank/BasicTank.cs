using OpenTK;
using System;


namespace GameLibrary.Tank
{

    public class BasicTank : AbstractTank
    {

        public BasicTank(Vector2 startPosition, int textrureID)
        {
            PositionCenter = startPosition;
            TankID = textrureID;
            Health = 100;
            Armor = 50;
            Ammo = 10;
            Speed = 0.1f; 
            Fuel = 2000; 
            IsShoot = false;
            gunOffset = new Vector2(0, 0f);
        }


        public override int Health { get; set; }

        public override int Armor { get; set; }

        public override int Fuel { get; set; }

        public override int Ammo { get; set; }

        public override float Speed { get; set; }

        public override int TankID { get; set; }

        public override int Direction { get; set; }

        public override Vector2 GetGunPosition()
        {
            // Позиция пушки относительно центра танка
            var gunPosition = PositionCenter + gunOffset;

            // Инвертируем X-координату пушки, если танк не стреляет
            if (!IsShoot)
            {
                gunPosition.X = PositionCenter.X - gunOffset.X;
            }

            return gunPosition;
        }

        public override void GetDamage(int damage)
        {
            if (Armor > 0)
            {
                int remainingDamage = damage - Armor;
                Armor = Math.Max(Armor - damage, 0);
                Health = Math.Max(Health - Math.Max(remainingDamage, 0), 0);
            }
            else
            {
                Health = Math.Max(Health - damage, 0);
            }
        }



        public override void Move(Vector2 movement)
        {
            if (IsMove || Fuel <= 0)
            {
                return;
            }

            PositionCenter += movement * Speed;
            Fuel = Math.Max(Fuel - 1, 0);
        }
    }
}
