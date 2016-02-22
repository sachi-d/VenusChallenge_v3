using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VenusGame
{
    class LifePack : GameEntity
    {
        int lifetime;
        public LifePack(int xx, int yy, int lt)
            : base(xx, yy)
        {
            x = xx;
            y = yy;
            lifetime = lt;
            playerName = "LP";
        }

        public int getLifeTime()
        {
            return lifetime;
        }
        public void setLifeTime(int c)
        {
            lifetime = c;
        }
        public void updateRandom()
        {
            //till lifetime expires - appear 
        }
    }
}
