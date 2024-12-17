using System.Drawing;
using OpenTK;

namespace GameLibrary
{
    public abstract class Bullet
    {
        public abstract int Damage { get; set; }
        public abstract float Speed { get; set; }
        protected int TextureID { get; set; }

        public Vector2 PositionCenter;

        public Vector2 MovementVector;


        public virtual void Fire()
        {
            PositionCenter += MovementVector;
        }
       
        public virtual RectangleF GetCollider()
        {
            Vector2[] colliderPosition = CalculateVertices();

            float colliderWidth = (colliderPosition[2].X - colliderPosition[3].X) / 2.0f;
            float colliderHeight = (colliderPosition[3].Y - colliderPosition[0].Y) / 2.0f;

            float[] normalizedTopLeft = NormalizeCoordinates(colliderPosition[3].X, colliderPosition[3].Y);

            RectangleF collider = new RectangleF(normalizedTopLeft[0], normalizedTopLeft[1], colliderWidth, colliderHeight);

            return collider;
        }
        protected static float[] NormalizeCoordinates(float x, float y)
        {
            float screenCenterX = 0.5f;
            float screenCenterY = 0.5f;

            float[] normalizedPoint = new float[2];

            normalizedPoint[0] = screenCenterX + x / 2.0f;
            normalizedPoint[1] = screenCenterY - y / 2.0f;

            return normalizedPoint;
        }
        public virtual Vector2[] CalculateVertices()
        {
            return new Vector2[4]
            {
                PositionCenter + new Vector2(-0.05f, -0.03f),
                PositionCenter + new Vector2(0.05f, -0.03f),
                PositionCenter + new Vector2(0.05f, 0.03f),
                PositionCenter + new Vector2(-0.05f, 0.03f),
            };
        }
        
        public virtual void Render()
        {
            
                ObjectRender.RenderObjects(TextureID, CalculateVertices());

        }
    }
}
