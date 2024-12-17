using GameLibrary;
using OpenTK;

namespace PrizesLibrary.Prizes
{
    public class SpeedDeBoostPrize : Prize
    {
        public SpeedDeBoostPrize(Vector2 centerPosition, int inMapPosition)
        {
            this.textureID = CreateTexture.LoadTexture("degrade.png"); ;
            this.centerPosition = centerPosition;
            this.inMapPosition = inMapPosition;
        }
    }
}
