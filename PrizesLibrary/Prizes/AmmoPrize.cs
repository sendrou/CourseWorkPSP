using GameLibrary;
using OpenTK;

namespace PrizesLibrary.Prizes
{

    public class AmmoPrize : Prize
    {

        public AmmoPrize(Vector2 centerPosition,int inMapPosition)
        {
            this.textureID = CreateTexture.LoadTexture("ammoPrize.png");
            this.centerPosition = centerPosition;
            this.inMapPosition = inMapPosition;
        }
    }
}
