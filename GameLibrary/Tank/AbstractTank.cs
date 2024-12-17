using OpenTK;
using System.Drawing;

namespace GameLibrary.Tank
{
    public abstract class AbstractTank
    {
        public abstract int TankID { get; set; }

        public Vector2 PositionCenter;

        protected Vector2 gunOffset;

        public Vector2 velocity;


        public bool IsMove { get; set; }
        public bool IsShoot { get; set; }
        public bool IsWindWork { get; set; }
        
        public abstract int Health { get; set; }
        public abstract int Armor { get; set; }
        public abstract int Fuel { get; set; }
        public abstract int Ammo { get; set; }
        public abstract float Speed { get; set; }
        public abstract int Direction { get; set;}

        private const int _maxAmmo = 20;
        private const int _maxHealth = 200;
        private const int _maxArmor = 100;
        private const int _maxFuel = 3000;
        public abstract void GetDamage(int damage);

        public abstract Vector2 GetGunPosition();
        public abstract void Move(Vector2 movement);

        public virtual RectangleF GetCollider()
        {
            Vector2[] colliderPosition = CalculateVertices();

            float colliderWidth = (colliderPosition[2].X - colliderPosition[3].X) / 2.0f;
            float colliderHeight = (colliderPosition[3].Y - colliderPosition[0].Y) / 2.0f;

            float[] convertedLeftTop = NormalizeCoordinates(colliderPosition[3].X, colliderPosition[3].Y);

            RectangleF collider = new RectangleF(convertedLeftTop[0], convertedLeftTop[1], colliderWidth - 0.005f, colliderHeight - 0.03f);

            return collider;
        }
        public virtual float[] NormalizeCoordinates(float pointX, float pointY)
        {
            float centralPointX = 0.5f;
            float centralPointY = 0.5f;

            float[] resultPoint = new float[2];

            resultPoint[0] = centralPointX + pointX / 2.0f;
            resultPoint[1] = centralPointY - pointY / 2.0f;

            return resultPoint;
        }
        protected virtual Vector2[] CalculateVertices()
        {
            return new Vector2[4]
           {
                PositionCenter + new Vector2(-0.07f, -0.12f),
                PositionCenter + new Vector2(0.07f, -0.12f),
                PositionCenter + new Vector2(0.07f, 0.12f),
                PositionCenter + new Vector2(-0.07f, 0.12f),
           };
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }
        public void SetPosition(Vector2 newPosition)
        {
            PositionCenter = newPosition;
        }
        

        public virtual void Render()
        {
            ObjectRender.RenderObjects(TankID, CalculateVertices());
        }
    }
}
