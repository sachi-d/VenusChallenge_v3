using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VenusGame
{
    class Stone : GameEntity
    {
        public Stone(int xx, int yy)
            : base(xx, yy)
        {
            playerName = "SS";
        }
    }
}
