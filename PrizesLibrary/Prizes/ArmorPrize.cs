using GameLibrary;
using OpenTK;

namespace PrizesLibrary.Prizes
{

    public class ArmorPrize : Prize
    {

        public ArmorPrize(Vector2 centerPosition, int inMapPosition)
        {
            this.textureID = CreateTexture.LoadTexture("armorPrize.png");
            this.centerPosition = centerPosition;
            this.inMapPosition = inMapPosition;
        }
    }
}
