using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTankGame
{
    abstract class GameEntity
    {
        public int x; //horizontal position
        public int y; //vertical position
        public string playerName;
        public GameEntity(int xx, int yy)
        {
            x = xx;
            y = yy;
        }
        public GameEntity()
        {
            x = 0;
            y = 0;
            playerName = "";
        }

        public override string ToString()
        {
            return playerName;
        }
    }
}