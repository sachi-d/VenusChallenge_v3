using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTankGame
{
    class LifePack : GameEntity
    {
        public int lifetime;
        public LifePack(int xx, int yy, int lt)
            : base(xx, yy)
        {
            x = xx;
            y = yy;
            lifetime = lt;
            playerName = "LP";
        }
        public void updateRandom()
        {
            //till lifetime expires - appear 
        }
    }
}
