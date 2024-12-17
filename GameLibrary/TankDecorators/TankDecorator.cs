using System.Drawing;
using GameLibrary.Tank;
using OpenTK;

namespace GameLibrary.TankDecorators
{
    public abstract class TankDecorator : AbstractTank
    {
        protected AbstractTank _tank;

        public TankDecorator(AbstractTank tank)
        {
            _tank = tank;
        }

        public override int Health
        {
            get { return _tank.Health; }
            set { _tank.Health = value; }
        }
        public override int Armor
        {
            get { return _tank.Armor; }
            set { _tank.Armor = value; }
        }
        public override int Fuel
        {
            get { return _tank.Fuel; }
            set { _tank.Fuel = value; }
        }
        public override int Ammo
        {
            get { return _tank.Ammo; }
            set { _tank.Ammo = value; }
        }
        public override float Speed
        {
            get { return _tank.Speed; }
            set { _tank.Speed = value; }
        }
        public override int TankID
        {
            get { return _tank.TankID; }
            set { _tank.TankID = value; }
        }


        public override int Direction 
        {
            get { return _tank.Direction; }
            set { _tank.Direction = value; }
        }

        public override void GetDamage(int damage)
        {
            _tank.GetDamage(damage);
        }


        public override Vector2 GetGunPosition()
        {
            Vector2 gunPosition = _tank.GetGunPosition() + gunOffset;

            if (!IsShoot)
            {
                gunPosition.X = _tank.GetGunPosition().X - gunOffset.X;
            }

            return gunPosition;
        }
        public override RectangleF GetCollider()
        {
            return _tank.GetCollider();
        }
        public override void Move(Vector2 movement)
        {
            _tank.Move(movement);
        }
        
        
        public override void Render()
        {
            _tank.Render();
        }

    }
}
