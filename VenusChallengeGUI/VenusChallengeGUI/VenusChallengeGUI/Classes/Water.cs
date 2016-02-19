using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTankGame
{
    class Water : GameEntity
    {
        public Water(int xx, int yy)
            : base(xx, yy)
        {
            playerName = "WW";
        }
    }
}
