﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VenusChallengeGUI
{
    class Tank : GameEntity
    {

        public int prevX;
        public int prevY;
        public int health;
        public int coins;
        public int points;
        public bool whetherShot;
        public bool status;
        public float angle;
        public int direction;
        public Vector2 dirpos;
        GameGrid grid;//the game grid this tank belongs to
        String respond;
        public Tank()
            : base()
        {
            angle = 0;
            coins = 0;
            points = 0;
            whetherShot = false;
            health = 0;
            status = true;
            respond = "";

        }
        public Tank(String l)
            : base()
        {

            coins = 0;
            points = 0;
            whetherShot = false;
            health = 0;
            status = true;
            respond = "";
            playerName = l;

        }
        public void setGrid(GameGrid g)
        {
            this.grid = g;
        }
        public GameEntity[,] getGrid()
        {
            return this.grid.GetGrid();
        }
        public void setPlayerName(String l)
        {
            playerName = l;
        }
        public void setDirection(int dir)
        {
            angle = dir * (MathHelper.ToRadians(90));
            direction = dir;
            int m, n;
            switch (dir)
            {
                case 0:
                    m = 0;
                    n = -1;
                    break;
                case 1:
                    m = +1;
                    n = 0;
                    break;
                case 2:
                    m = 0;
                    n = +1;
                    break;
                case 3:
                    m = -1;
                    n = 0;
                    break;
                default:
                    m = 0;
                    n = 0;
                    break;
            }
            dirpos = new Vector2(m, n);
        }

        

        public void setGridLocation(int newx, int newy)
        {
            int prex = newx;
            int prey = newy;

            switch (direction)   //change the cell the tank was previously residing to a normal cell 0123=NESW
            {
                case 0:
                    prey = newy + 1;
                    break;
                case 1:
                    prex = newx - 1;
                    break;
                case 2:
                    prey = newy - 1;
                    break;
                case 3:
                    prex = newx + 1;
                    break;
            }
            if (!(newx < 0 || newx > 9 || newy > 9 || newy < 0))
            {
                grid.GetGrid()[newx, newy] = this;
            }
            if (!(prex < 0 || prex > 9 || prey > 9 || prey < 0))
            {
                grid.GetGrid()[prex, prey] = new GameEntity();   //celltexture
            }
            x = newx;
            y = newy;
        }
        public void globalUpdate(String updatedValues)
        {
            Console.WriteLine("glovbal method updating");
            string[] c = updatedValues.Split(';');

            direction = Int32.Parse(c[2]);
            Console.WriteLine("updated direction");
            if (Int32.Parse(c[3]) != 0)
            {
                whetherShot = true;
            }
            health = Int32.Parse(c[4]);
            coins = Int32.Parse(c[5]);
            points = Int32.Parse(c[6]);
            Console.WriteLine("name- -" + playerName + "health- -" + health + "coins- -" + coins + "points - " + points + "");
        }



        public void move(string command)
        { // get the command and check the irection of the tank facing
            int todirection = 0;
            int tox = x;
            int toy = y;

            if (command.Equals("UP#"))
            {
                todirection = 0;
                toy -= 1;
            }
            else if (command.Equals("DOWN#"))
            {
                todirection = 2;
                toy += 1;
            }
            else if (command.Equals("RIGHT#"))
            {
                todirection = 1;
                tox += 1;
            }
            else if (command.Equals("LEFT#"))
            {
                todirection = 3;
                tox -= 1;
            }


            if (todirection == direction)
            {
                //check if inside the grid
                if (tox < 0 || toy < 0 || tox > 9 || toy > 9)
                {
                    Console.WriteLine("Going out of the grid");
                }
                else
                {
                    //string m = grid.gameGrid[tox, toy].ToString().Substring(0, 2);
                    //if (m.Equals("BB") || m.Equals("SS"))
                    //{
                    //    //Console.WriteLine("You are dead!!!!");
                    //}
                    //else if (m.Equals("WW"))
                    //{
                    //    Console.WriteLine("You are dead!!!!");
                    //}
                    //else if (m.Equals("CC"))
                    //{
                    //    this.coins++;
                    //    setGridLocation(tox, toy, direction);
                    //}
                    //else if (m.Equals("LP"))
                    //{
                    //    this.health++;
                    //    setGridLocation(tox, toy, direction);
                    //}


                    switch (grid.GetGrid()[tox, toy].ToString().Substring(0, 2))
                    {
                        case "BB":
                        case "SS":
                        case "PP":
                            break;
                        case "WW":
                            Console.WriteLine("You are dead!!!!");
                            break;
                        case "CC":
                            this.coins++;
                            //setGridLocation(tox, toy, direction);
                            break;
                        case "LP":
                            this.health++;
                            //setGridLocation(tox, toy, direction);
                            break;
                        default:
                            //setGridLocation(tox, toy, direction);
                            break;
                    }
                }


            }
            else
            {
                setDirection(todirection);
            }
        }
        public String respondCommands(String x)
        {
            x = x.Split('#')[0];
            if (x == "OBSTACLE")
            {
                respond = "Obstacle found in moved direction";
                Console.WriteLine("Obstacle found in moved direction");
                return respond;
            }
            else if (x == "CELL_OCCUPIED")
            {
                respond = "Tried to move to a occupied cell";
                Console.WriteLine("Tried to move to a occupied cell");
                return respond;
            }
            else if (x == "DEAD")
            {
                respond = "Player dead";
                Console.WriteLine("Player dead");
                return respond;
            }
            else if (x == "TOO_QUICK")
            {
                respond = "Slow down movements";
                Console.WriteLine("Slow down movements");
                return respond;
            }
            else if (x == "INVALID_CELL")
            {
                respond = "Not a valid cell";
                Console.WriteLine("Not a valid cell");
                return respond;
            }
            else if (x == "GAME_HAS_FINISHED")
            {
                respond = "Game end";
                Console.WriteLine("Game end");
                return respond;
            }
            else if (x == "PITFALL")
            {
                respond = "Pitfall - Game end";
                Console.WriteLine("Pitfall - Game end");
                return respond;
            }
            else if (x == "GAME_NOT_STARTED_YET")
            {
                respond = "Wait!Game will start in few seconds ";
                Console.WriteLine("Wait!Game will start in few seconds ");
                return respond;
            }
            else if (x == "NOT_A_VALID_CONTESTANT")
            {
                respond = "Only valid contestants are allowed";
                Console.WriteLine("Only valid contestants are allowed");
                return respond;
            }
            else
            {
                respond = "Not a valid respond";
                // Console.WriteLine("Not a valid respond");
                return respond;

            }
        }

        public override string ToString()
        {
            return "PP" + playerName;
        }
        public int getPlayerDigit()
        {
            return Int32.Parse(this.playerName);
        }

    }

    class MyTank : Tank
    {
        bool isShooting;
        public Vector2 bulletpos;

        public void setShooting(bool m)
        {
            isShooting = m;
        }
        public bool getShooting()
        {
            return isShooting;
        }

        public int getShootLength()
        {
            int count = 0;
            Vector2 nn = new Vector2(x, y) + dirpos;

            while (count <= 9 && nn.X<=9 && nn.Y<=9 && nn.X>=0 && nn.Y>=0)
            {
                string s=this.getGrid()[(int)nn.X, (int)nn.Y].ToString().Substring(0,2);
                if (s.Equals("BB") || s.Equals("PP") || s.Equals("SS"))
                {
                    break;
                }
                else
                {
                    count++;
                    if (dirpos.X != 0)
                    {
                        nn.X++;
                    }
                    else
                    {
                        nn.Y++;
                    }
                }
            }
            return count;
        }

        public override string ToString()
        {
            return "PPMY";
        }

    }




}