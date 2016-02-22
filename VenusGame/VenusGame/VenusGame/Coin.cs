using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VenusGame
{
    class Coin : LifePack
    {
        public int value;

        public Coin(int xx, int yy, int lt, int val)
            : base(xx, yy, lt)
        {
            x = xx;
            y = yy;
            this.setLifeTime(lt);
            value = val;
            playerName = "CC";
        }
        public int getCoinValue(int x, int y)
        {
            return value;
        }

    }
}
