using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VenusChallengeGUI
{
    class Brick : GameEntity
    {
        int health;
        public Brick(int xx, int yy)
            : base(xx, yy)
        {
            health = 100;
            playerName = "BB";
        }
        public void updateDamage()
        {

        }
        public int getHealth()
        {
            return health;
        }
        public void setHealth(int h)
        {
            this.health = h;
        }
    }
}
