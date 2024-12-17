using GameLibrary;
using OpenTK;

namespace PrizesLibrary.Prizes
{

    public class HealthPrize : Prize
    {

        public HealthPrize(Vector2 centerPosition, int inMapPosition)
        {
            this.textureID = CreateTexture.LoadTexture("healthPrize.png");
            this.centerPosition = centerPosition;
            this.inMapPosition = inMapPosition;
        }

    }
}
