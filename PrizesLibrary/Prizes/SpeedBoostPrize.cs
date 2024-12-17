using GameLibrary;
using OpenTK;

namespace PrizesLibrary.Prizes
{
    public class SpeedBoostPrize : Prize
    {

        public SpeedBoostPrize(Vector2 centerPosition, int inMapPosition)
        {
            this.textureID = CreateTexture.LoadTexture("upgrade.png"); ;
            this.centerPosition = centerPosition;
            this.inMapPosition = inMapPosition;
        }

    }
}
