using GameLibrary;
using OpenTK;
using System.Drawing;

namespace PrizesLibrary.Prizes
{

    public abstract class Prize
    {
        protected Vector2 centerPosition;
        protected int textureID;
        public int inMapPosition;

        
        public RectangleF GetCollider()
        {
            Vector2[] colliderPosition = CalculateVertices();

            float colliderWidth = (colliderPosition[2].X - colliderPosition[3].X) / 2.0f;
            float colliderHeight = (colliderPosition[3].Y - colliderPosition[0].Y) / 2.0f;

            float[] convertedLeftTop = NormalizeCoordinates(colliderPosition[3].X, colliderPosition[3].Y);

            RectangleF collider = new RectangleF(convertedLeftTop[0], convertedLeftTop[1], colliderWidth - 0.005f, colliderHeight - 0.03f);

            return collider;
        }
        private static float[] NormalizeCoordinates(float pointX, float pointY)
        {
            float centralPointX = 0.5f;
            float centralPointY = 0.5f;

            float[] resultPoint = new float[2];

            resultPoint[0] = centralPointX + pointX / 2.0f;
            resultPoint[1] = centralPointY - pointY / 2.0f;

            return resultPoint;
        }

        protected Vector2[] CalculateVertices()
        {
            return new Vector2[4]
          {
                centerPosition + new Vector2(-0.05f, -0.1f),
                centerPosition + new Vector2(0.05f, -0.1f),
                centerPosition + new Vector2(0.05f, 0.1f),
                centerPosition + new Vector2(-0.05f, 0.1f),
          };
        }

        public void Render()
        {
            ObjectRender.RenderObjects(textureID, CalculateVertices());
        }

    }
}
