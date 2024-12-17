using GameLibrary.Tank;
using System;
using System.Threading.Tasks;

namespace GameLibrary.TankDecorators
{
    public class SpeedDeBoostDecorator : TankDecorator
    {
        private float _extraSpeed;
        private const int DurationInSeconds = 5;

        public SpeedDeBoostDecorator(AbstractTank tank, float extraSpeed) : base(tank)
        {
            _extraSpeed = extraSpeed;
            tank.Speed = CalculateSpeed(tank.Speed, _extraSpeed);

            RestoreSpeedAsync(tank, _extraSpeed);
        }

        private float CalculateSpeed(float currentSpeed, float extraSpeed)
        {
            return currentSpeed - extraSpeed;
        }

        private async void RestoreSpeedAsync(AbstractTank tank, float extraSpeed)
        {
            await Task.Delay(TimeSpan.FromSeconds(DurationInSeconds));

            tank.Speed += extraSpeed;
        }

        public override float Speed
        {
            get => _tank.Speed;
            set => _tank.Speed = value;
        }
    }
}
