using GameLibrary.Tank;
using GameLibrary.TankDecorators;
using PrizesLibrary.Prizes;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TankBattle.Managers
{
    public class PrizeManager : IGameObjectManager
    {
        private readonly Random _random;
        private NetworkManager _networkManager;
        private GameManager _gameManager;

        private const int MaxHealth = 200;
        private const int MaxArmor = 50;
        private const int MaxFuel = 3000;
        private const int MaxAmmo = 30;
        private const float MaxSpeed = 0.15f;

        private float _previousSpeed;
        private int _lastPrizePosition = -1;

        public PrizeManager()
        {
            _random = new Random();
        }

        public void SetManagers(NetworkManager networkManager, GameManager gameManager)
        {
            _networkManager = networkManager;
            _gameManager = gameManager;
        }

        public void PrizeTimer_Tick(object sender, EventArgs e)
        {
            // Ensure that there are fewer than 5 prizes on the field
            if (_networkManager.CurrentPrizeList.Count < 5)
            {
                Prize newPrize = _networkManager.PrizeFactory.AddNewPrize(_lastPrizePosition);
                _lastPrizePosition = -1;

                _networkManager.CurrentPrize = newPrize;
                _networkManager.CurrentPrizeList.Add(_networkManager.CurrentPrize);
            }
        }

        public void ApplyPrize(List<Prize> prizeList, AbstractTank player)
        {
            foreach (var prize in prizeList.ToList())
            {
                // Set the current prize to the one we are checking
                _networkManager.CurrentPrize = prize;

                // Check if the player intersects with the prize
                if (player.GetCollider().IntersectsWith(_networkManager.CurrentPrize.GetCollider()))
                {
                    ApplyPrizeEffect(player, prize);
                    prizeList.Remove(prize);  // Remove the prize once applied
                }
            }
        }

        private void ApplyPrizeEffect(AbstractTank player, Prize prize)
        {
            switch (prize)
            {
                case AmmoPrize ammoPrize:
                    ApplyAmmoPrize(player);
                    break;

                case ArmorPrize armorPrize:
                    ApplyArmorPrize(player);
                    break;

                case FuelPrize fuelPrize:
                    ApplyFuelPrize(player);
                    break;

                case HealthPrize healthPrize:
                    ApplyHealthPrize(player);
                    break;

                case SpeedBoostPrize speedBoostPrize:
                    ApplySpeedBoostPrize(player);
                    break;

                case SpeedDeBoostPrize speedDeBoostPrize:
                    ApplySpeedDeBoostPrize(player);
                    break;

                default:
                    throw new ArgumentException($"Unknown prize type: {prize.GetType().Name}");
            }
        }

        private void ApplyAmmoPrize(AbstractTank player)
        {
            int ammoBoostCount = 5;
            player = new AmmoBoostDecorator(player, ammoBoostCount);
            _lastPrizePosition = _networkManager.CurrentPrize.inMapPosition;
        }

        private void ApplyArmorPrize(AbstractTank player)
        {
            int armorBoostCount = 15;
            player = new ArmorBoostDecorator(player, armorBoostCount);
            _lastPrizePosition = _networkManager.CurrentPrize.inMapPosition;
        }

        private void ApplyFuelPrize(AbstractTank player)
        {
            int fuelBoostCount = 500;
            player = new FuelBoostDecorator(player, fuelBoostCount);
            _lastPrizePosition = _networkManager.CurrentPrize.inMapPosition;
        }

        private void ApplyHealthPrize(AbstractTank player)
        {
            int healthBoostCount = 20;
            player = new HealthBoostDecorator(player, healthBoostCount);
            _lastPrizePosition = _networkManager.CurrentPrize.inMapPosition;
        }

        private void ApplySpeedBoostPrize(AbstractTank player)
        {
            float speedBoostCount = 0.015f;
            player = new SpeedBoostDecorator(player, speedBoostCount);
            _lastPrizePosition = _networkManager.CurrentPrize.inMapPosition;
        }

        private void ApplySpeedDeBoostPrize(AbstractTank player)
        {
            float speedDeBoostCount = 0.015f;
            player = new SpeedDeBoostDecorator(player, speedDeBoostCount);
            _lastPrizePosition = _networkManager.CurrentPrize.inMapPosition;
        }
    }
}
