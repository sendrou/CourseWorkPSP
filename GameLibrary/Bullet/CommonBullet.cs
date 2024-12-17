using OpenTK;

namespace GameLibrary
{
    public class CommonBullet : Bullet
    {
        public override int Damage { get; set; } = 20;
        public override float Speed { get; set; } = 0.02f;

        public CommonBullet(Vector2 startPosition, int textureLeftID, int textureRightID, int textureUpID, int textureDownID, int movementDirection) : base()
        {
            PositionCenter = startPosition;
            
            switch (movementDirection)
            {
                case 0:
                    // Up
                    this.MovementVector = new Vector2(0f, -Speed); 
                    TextureID = textureUpID;
                    PositionCenter = PositionCenter - new Vector2(0f, 0.1f);
                    break;
                case 1:
                    // Down
                    this.MovementVector = new Vector2(0f, Speed); 
                    TextureID = textureDownID;
                    PositionCenter = PositionCenter + new Vector2(0f, 0.1f);
                    break;

                case 2:
                    // Left
                    this.MovementVector = new Vector2(-Speed, 0f); 
                    PositionCenter = PositionCenter -new Vector2(0.1f, 0f); 
                    TextureID = textureLeftID;
                    break;

                case 3:
                    // Right
                    this.MovementVector = new Vector2(Speed, 0f); 
                    TextureID = textureRightID;
                    PositionCenter = PositionCenter + new Vector2(0.1f, 0f);
                    break;
            }
        }
        public override Vector2[] CalculateVertices()
        {
            return new Vector2[4]
            {
                PositionCenter + new Vector2(-0.03f, -0.04f),
                PositionCenter + new Vector2(0.03f, -0.04f),
                PositionCenter + new Vector2(0.03f, 0.04f),
                PositionCenter + new Vector2(-0.03f, 0.04f),
            };
        }

    }
}
