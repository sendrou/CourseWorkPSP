using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle.Managers
{
    public interface IGameObjectManager
    {
        void SetManagers(NetworkManager manager, GameManager gameManager);
    }
}
