using System;
using System.Drawing;
using OpenTK;
using PrizesLibrary.Prizes;

namespace PrizesLibrary.Factories
{

    public class PrizeFactory
    {
        Random random;

        Vector2[] positions = new Vector2[]
                    {
                        new Vector2(-0.6f, -0.9f),
                        new Vector2(-0.8f, 0.8f),
                        new Vector2(-0.4f, 0.8f),
                        new Vector2(0.8f, 0.8f),
                        new Vector2(0.8f, -0.9f),
                        new Vector2(0.1f, -0.6f),
                        new Vector2(-0.25f, -0.8f)
                    };

        bool[] busyPositions = new bool[7] { false, false, false, false, false, false, false };
        public PrizeFactory(Random random)
        {
            this.random = random;

        }

        public Prize AddNewPrize(int lastPrizePozition)
        {
            
            Prize prize = null;
            int prizeNumber = random.Next(0, 6);
            int positionNumber = random.Next(0, 7);
            while( busyPositions[positionNumber])
            { 
                positionNumber = random.Next(0, 7);
            }
            busyPositions[positionNumber] = true;
            if (lastPrizePozition != -1)
                busyPositions[lastPrizePozition] = false;
            switch (prizeNumber)
                {
                    case 0:
                        prize = new AmmoPrize(positions[positionNumber], positionNumber);
                        break;
                    case 1:
                        prize = new ArmorPrize(positions[positionNumber], positionNumber);
                        break;
                    case 2:
                        prize = new HealthPrize(positions[positionNumber], positionNumber);
                        break;
                    case 3:
                        prize = new SpeedBoostPrize(positions[positionNumber], positionNumber);
                        break;
                    case 4:
                        prize = new FuelPrize(positions[positionNumber], positionNumber);
                        break;
                    case 5:
                        prize = new SpeedDeBoostPrize(positions[positionNumber], positionNumber);
                        break;
                    default:
                        break;
                }
                return prize;
            
            
        }
    }
}
