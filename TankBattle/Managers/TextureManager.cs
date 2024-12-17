using GameLibrary;
using System;

namespace TankBattle.Managers
{
    public static class TextureManager
    {
        public static int commonBulletDownTexture { get; private set; }
        public static int commonBulletLeftTexture { get; private set; }
        public static int commonBulletRightTexture { get; private set; }
        public static int commonBulletUpTexture { get; private set; }

        public static int firstTankTextureRight { get; private set; }

        public static int firstTankTextureUP { get; private set; }
        public static int firstTankTextureDown { get; private set; }
        public static int firstTankTextureLeft { get; private set; }
        public static int secondTankTextureRight { get; private set; }
        public static int secondTankTextureLeft { get; private set; }
        public static int secondTankTextureUP { get; private set; }
        public static int secondTankTextureDown { get; private set; }
        public  static int backGroundTexture { get; private set; }
        public static int mapBackGroundTexture { get; private set; }

        public static int brickCenterTexture { get; private set; }

        public static void SetupTexture()
        {
            firstTankTextureRight = CreateTexture.LoadTexture("firstTankRight.png");
            firstTankTextureLeft = CreateTexture.LoadTexture("firstTankLeft.png");
            firstTankTextureUP = CreateTexture.LoadTexture("firstTankUP.png");
            firstTankTextureDown = CreateTexture.LoadTexture("firstTankDown.png");

            secondTankTextureRight = CreateTexture.LoadTexture("secondTankRight.png");
            secondTankTextureLeft = CreateTexture.LoadTexture("secondTankLeft.png");
            secondTankTextureUP = CreateTexture.LoadTexture("secondTankUP.png");
            secondTankTextureDown = CreateTexture.LoadTexture("secondTankDown.png");


            commonBulletDownTexture = CreateTexture.LoadTexture("CommonBulletDown.png");
            commonBulletUpTexture = CreateTexture.LoadTexture("CommonBulletUp.png");
            commonBulletLeftTexture = CreateTexture.LoadTexture("CommonBulletLeft.png");
            commonBulletRightTexture = CreateTexture.LoadTexture("CommonBulletRight.png");



            brickCenterTexture = CreateTexture.LoadTexture("grey_brick_center.png");




            backGroundTexture = CreateTexture.LoadTexture("menu.png");
            mapBackGroundTexture = CreateTexture.LoadTexture("grass.png");
        }


    }
}
