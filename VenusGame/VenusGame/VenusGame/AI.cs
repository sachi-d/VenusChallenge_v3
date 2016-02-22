using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace VenusGame
{
    class AI
    {

        public bool gameOver = false;
        Client2 client;
        private Timer _pingTimer;
        GameGrid grid;
        int count;
        int score;

        int scoreChild;
        Tank tank;
        GameEntity cell;
        List<GameEntity> children;
        List<GameEntity> tankList;
        List<GameEntity> emptyCells;
        List<GameEntity> final;
        Random rnd;
        bool county;
        String h;
        public AI(Game1 g, Client2 cli)
        {

            this._pingTimer = new Timer();
            this._pingTimer.Interval = 100;
            this._pingTimer.Elapsed += new ElapsedEventHandler(this.TimeElapsed);

            //trial run
            //   this._pingTimer.Elapsed += new ElapsedEventHandler(this.TrialRun);


            this._pingTimer.Enabled = true;

            this.client = cli;

            setGrid(g.Gamegrid);

            count = 0;
            score = 0;
            county = true;
            cell = new GameEntity();
            Random rnd = new Random();
        }



        // Configure a Timer for use

        public void setGrid(GameGrid g)
        {
            this.grid = g;
            setTank();
        }
        public GameGrid getGrid()
        {
            return grid;
        }


        public void setTank()
        {
            tank = grid.mytank;

        }



        public void TimeElapsed(Object sender, ElapsedEventArgs eventArgs)
        {

            List<GameEntity> result = new List<GameEntity>();
            result = GetBestScore();

            if (result[0].x.Equals(result[1].x))
            {
                if (result[0].y > result[1].y)
                {


                    if (tank.direction != 0)
                    {
                        county = false;
                        client.SendData("UP#");
                    }
                    //  client.SendData("UP#");
                    // client.startSend("UP#");
                }
                else
                {
                    if (tank.direction != 2)
                    {
                        client.SendData("DOWN#");
                    }
                    //  client.SendData("DOWN#");

                    //client.startSend("DOWN#");
                }
            }
            else if ((result[0].y).Equals(result[1].y))
            {
                if (result[0].x > result[1].x)
                {
                    if (tank.direction != 1)
                    {
                        client.SendData("RIGHT#");
                    }
                    //  client.SendData("RIGHT#");
                    //client.startSend("RIGHT#");
                }
                else
                {
                    if (tank.direction != 3)
                    {
                        client.SendData("LEFT#");
                    }
                    //client.SendData("LEFT#");
                    // client.startSend("LEFT#");
                }
            }
            Console.WriteLine(tank.x + " " + tank.y);

            //     grid.GetGrid()[cell.x, cell.y] = tank;
            //    grid.GetGrid()[cell.x, cell.y] = tank;

            //   Console.WriteLine(this.Iterate(startNode, int.MaxValue, int.MinValue, MaxPlayer)+"Final Score");
        }
        public List<GameEntity> Children()
        {
            for (int i = 0; i < 9999999999999999; i++)
            {
                if (grid.isGridSet)
                {
                    break;
                }
            }
            grid.displayGrid();
            children = new List<GameEntity>();
            tankList = new List<GameEntity>();
            emptyCells = new List<GameEntity>();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {

                    if ((grid.GetGrid()[i, j].ToString().Equals("CELL")) || (grid.GetGrid()[i, j].ToString().Equals("LP")) || (grid.GetGrid()[i, j].ToString().Equals("CC")))
                    {
                        if ((i == grid.mytank.x && j == grid.mytank.y - 1) || (i == grid.mytank.x + 1 && j == grid.mytank.y) || (i == grid.mytank.x && j == grid.mytank.y + 1) || (i == grid.mytank.x - 1 && j == grid.mytank.y))
                        {

                            Console.WriteLine(grid.GetGrid()[i, j].returnObj(i, j).x + " " + grid.GetGrid()[i, j].returnObj(i, j).y + "child");
                            children.Add(grid.GetGrid()[i, j].returnObj(i, j));
                        }
                        else
                        {
                            emptyCells.Add(grid.GetGrid()[i, j].returnObj(i, j));
                        }
                    }
                    else if (grid.GetGrid()[i, j].ToString().Substring(0, 2).Equals("PP"))
                    {
                        tankList.Add(grid.GetGrid()[i, j].returnObj(i, j));
                    }
                    else
                    {

                    }

                }
                // Console.WriteLine("\n");
            }


            return children;
        }

        public List<GameEntity> GetBestScore()
        {
            final = new List<GameEntity>();
            GameEntity cellprev = new GameEntity();
            int bestScore = 0;
            cellprev = cell;
            final.Add(cellprev);
            List<GameEntity> ne;
            ne = Children();
            for (int o = 0; o < ne.Count; o++)
            {
                Console.WriteLine("Childreb+" + ne[o].playerName + " " + ne.Count);
            }

            for (int i = 0; i < ne.Count; i++)
            {
                int totalScore = 0;
                Console.WriteLine(i + "  ");
                totalScore = evaluateScore(ne[i]);
                Console.WriteLine("total score" + totalScore);
                if (bestScore < totalScore)
                {
                    bestScore = totalScore;
                    cell = ne[i];
                    Console.WriteLine(ne[i].x + "  x " + ne[i].y);

                }


            }
            if ((cell.x.Equals(cellprev.x)) && (cell.y.Equals(cellprev.y)))
            {
                Console.WriteLine("Blah Blah Blah");
                int r = rnd.Next(ne.Count);
                cell = ne[r];
            }
            score = bestScore;
            final.Add(cell);
            Console.WriteLine("best score" + bestScore);
            return final;

        }
        public int evaluateScore(GameEntity node)
        {
            count = 0;
            scoreChild = 0;


            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.WriteLine("tank.x" + tank.x + " " + "tank.y" + tank.y + "node.x" + node.x + " " + "node.y" + node.y);

                    foreach (GameEntity c in tankList)
                    {
                        if (node.x == c.x || node.y == c.y)
                        {

                            scoreChild -= 1000;     //int possibleDeath=1000;  
                        }


                    }
                    if (emptyCells.Contains(grid.GetGrid()[i, j].returnObj(i, j)))
                    {
                        if ((i == node.x && i == node.x) || (j == node.y && j == node.y))
                        {

                            if (grid.GetGrid()[i, j].GetType() == typeof(LifePack))
                            {
                                scoreChild += 800;
                            }
                            else if (grid.GetGrid()[i, j].GetType() == typeof(Coin))
                            {
                                scoreChild += 750;
                            }
                            else
                            {
                                scoreChild += 700;
                            }
                        }


                        if ((i <= node.x + 1 && i >= node.x - 1) || (j <= node.y + 1 && j >= node.y - 1))
                        {

                            if (grid.GetGrid()[i, j].GetType() == typeof(LifePack))
                            {
                                scoreChild += 770;
                            }
                            else if (grid.GetGrid()[i, j].GetType() == typeof(Coin))
                            {
                                scoreChild += 730;
                            }
                            else
                            {
                                scoreChild += 600;
                            }
                        }

                        else if ((i <= node.x + 2 && i >= node.x - 2) || (j <= node.y + 2 && j >= node.y - 2))
                        {



                            if (grid.GetGrid()[i, j].GetType() == typeof(LifePack))
                            {
                                scoreChild += 680;
                            }
                            else if (grid.GetGrid()[i, j].GetType() == typeof(Coin))
                            {
                                scoreChild += 630;
                            }
                            else
                            {
                                scoreChild += 500;
                            }

                        }
                        else if ((i <= node.x + 3 && i >= node.x - 3) || (j <= node.y + 3 && j >= node.y - 3))
                        {


                            if (grid.GetGrid()[i, j].GetType() == typeof(LifePack))
                            {
                                scoreChild += 580;
                            }
                            else if (grid.GetGrid()[i, j].GetType() == typeof(Coin))
                            {
                                scoreChild += 540;
                            }
                            else
                            {
                                scoreChild += 400;
                            }


                        }

                        else
                        {


                            if (grid.GetGrid()[i, j].GetType() == typeof(LifePack))
                            {
                                scoreChild += 480;
                            }
                            else if (grid.GetGrid()[i, j].GetType() == typeof(Coin))
                            {
                                scoreChild += 420;
                            }
                            else
                            {
                                scoreChild += 300;
                            }
                        }
                    }
                    else
                    {
                        scoreChild -= 2000;
                    }




                }



            }

            Console.WriteLine("score child" + scoreChild);
            return scoreChild;

        }

    }


    //if (node.IsTerminal())
    //{int n = node.GetTotalScore(Player).Row;
    //int m = node.GetTotalScore(Player).Column;
    //    t.x = n;
    //    t.y = m;
    //    Console.WriteLine("sferfhgfj");
    //    return node.GetTotalScore(Player).score;
    //}

    //if (Player == MaxPlayer)
    //{
    //    foreach (Node child in node.Children(Player))
    //    {
    //        alpha = Math.Max(alpha, Iterate(child, alpha, beta, !Player));
    //        if (beta < alpha)
    //        {
    //            break;
    //        }

    //    }

    //    return alpha;
    //}
    //else
    //{
    //    foreach (Node child in node.Children(Player))
    //    {
    //        beta = Math.Min(beta, Iterate(child, alpha, beta, !Player));

    //        if (beta < alpha)
    //        {
    //            break;
    //        }
    //    }

    //    return beta;
    //}




}
