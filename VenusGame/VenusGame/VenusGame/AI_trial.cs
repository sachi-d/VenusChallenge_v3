using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace VenusGame
{
    class AI_trial
    {

        Client2 client;
        private Timer disTimer;
        Timer start;
        GameGrid grid;
        int score;
        int scoreChild;
        Tank tank;
        GameEntity cell;
        List<GameEntity> children;
        List<Tank> tankList;

        List<GameEntity> final;
        bool county;
        String previous;
        String shoot;
        bool breakk;
        int count;
        int m;
        public AI_trial(Game1 g, Client2 cli)
        {


            start = new Timer();
            start.Elapsed += start_Elapsed;
            start.Interval = 50;
            start.Enabled = true;
            this.client = cli;
            setGrid(g.Gamegrid);
            tank = grid.mytank;
            score = 0;
            m = 0;
            county = true;
            breakk = true;
            shoot = "";
            cell = new GameEntity();
            Random rnd = new Random();
            count = 0;
        }

        void start_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Wait for 5 seconds");

            start.Enabled = false;
            while (!client.started)
            {
                //Console.WriteLine("waiting-------------");
            }
            startsend();
        }


        public void setGrid(GameGrid g)
        {
            this.grid = g;
        }


        public void startsend()
        {
            disTimer = new System.Timers.Timer();
            disTimer.Elapsed += disTimer_Elapsed;
            disTimer.Interval = 2500;
            disTimer.Enabled = true;

        }

        public void disTimer_Elapsed(Object sender, ElapsedEventArgs eventArgs)
        {
            client.count = true;
            // disTimer.Stop();
            if (shoot != null)
            {

                client.updateSendMessage(shoot);
                shoot = null;
            }
            else if (county)
            {
                List<GameEntity> result = new List<GameEntity>();
                result = GetBestScore();

                if ((result[0].x.Equals(result[1].x)) && (result[0].y > result[1].y))
                {
                    if (tank.direction == 0)
                    {
                        county = true;
                        previous = null;
                        client.updateSendMessage("UP#");
                    }
                    else
                    {
                        county = false;
                        previous = "UP#";
                        client.updateSendMessage("UP#");
                    }
                }
                else if ((result[0].x.Equals(result[1].x)) && (result[0].y < result[1].y))
                {
                    if (tank.direction == 1)
                    {
                        county = true;
                        previous = null;
                        client.updateSendMessage("DOWN#");
                    }
                    else
                    {
                        county = false;
                        previous = "DOWN#";
                        client.updateSendMessage("DOWN#");
                    }
                }
                else if ((result[0].y.Equals(result[1].y)) && (result[0].x < result[1].x))
                {
                    if (tank.direction == 2)
                    {
                        county = true;
                        previous = null;
                        client.updateSendMessage("RIGHT#");
                    }
                    else
                    {
                        county = false;
                        previous = "RIGHT#";
                        client.updateSendMessage("RIGHT#");

                    }
                }
                else if ((result[0].y.Equals(result[1].y)) && (result[0].x > result[1].x))
                {
                    if (tank.direction == 3)
                    {
                        county = true;
                        previous = null;
                        client.updateSendMessage("LEFT#");
                    }
                    else
                    {
                        county = false;
                        previous = "LEFT#";
                        client.updateSendMessage("LEFT#");
                    }
                }
            }
            else
            {
                client.updateSendMessage(previous);
                Console.WriteLine("Previous - " + previous + "..............");
                previous = null;
                county = true;
            }
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
            tankList = new List<Tank>();
            tankList = grid.tankList;




            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {


                    if ((i == grid.mytank.x && j == grid.mytank.y - 1) || (i == grid.mytank.x + 1 && j == grid.mytank.y) || (i == grid.mytank.x && j == grid.mytank.y + 1) || (i == grid.mytank.x - 1 && j == grid.mytank.y))
                    {
                        if ((grid.GetGrid()[i, j].ToString().Equals("CELL")) || (grid.GetGrid()[i, j].ToString().Equals("LP")) || (grid.GetGrid()[i, j].ToString().Equals("CC")))
                        {

                            Console.WriteLine(grid.GetGrid()[i, j].returnObj(i, j).x + " " + grid.GetGrid()[i, j].returnObj(i, j).y + "child");
                            children.Add(grid.GetGrid()[i, j].returnObj(i, j));
                        }



                    }


                }
                // Console.WriteLine("\n");
            }
            return children;
        }

        public List<GameEntity> GetBestScore()
        {
            int count = 0;
            bool val = true;
            int h = 0;
            int d = 0;
            final = new List<GameEntity>();
            GameEntity cellprev = new GameEntity();
            int bestScore = 0;
            cellprev = cell;
            final.Add(cellprev);
            List<GameEntity> ne;
            List<int> me = new List<int>();
            ne = Children();
            for (int i = 0; i < ne.Count; i++)
            {
                int totalScore = 0;
                //    Console.WriteLine(i + "  ");
                totalScore = evaluateScore(ne[i]);
                Console.WriteLine("total score" + totalScore);
                if (totalScore == -10000)
                {
                    // val = false;
                    //   shoot = "SHOOT#";
                    bestScore = totalScore;
                    cell = ne[i];
                    break;
                }
                else if (totalScore == 1000)
                {
                    count++;
                    bestScore = totalScore;
                    cell = ne[i];
                    Console.WriteLine("current best score" + ne[i].x + "  x " + ne[i].y);
                    break;
                }
                else if (totalScore == 750)
                {
                    count++;
                    bestScore = totalScore;
                    cell = ne[i];
                    Console.WriteLine("current best score" + ne[i].x + "  x " + ne[i].y);
                    break;
                }
                else if (totalScore > bestScore)
                {
                    count++;
                    bestScore = totalScore;
                    cell = ne[i];
                    me.Add(i);
                    Console.WriteLine("current best score" + ne[i].x + "  x " + ne[i].y);
                    //  break;
                }
                else if (totalScore == bestScore)
                {
                    me.Add(i);
                    Random rnd = new Random();
                    m = rnd.Next(0, me.Count);
                    cell = ne[m];
                }
                
            }

            score = bestScore;
            final.Add(cell);
            Console.WriteLine("best score" + bestScore);
            return final;
        }


        public int evaluateScore(GameEntity node)
        {

            scoreChild = 0;
            if (count == 0)
            {
                foreach (GameEntity c in tankList)
                {
                    if (tank.x == c.x)
                    {

                        if (((tank.y > c.y) && (tank.direction == 1)) || ((tank.y < c.y) && (tank.direction == 0)))
                        {
                            scoreChild = -10000;
                            shoot = "SHOOT#";
                            break;
                        }
                        else if ((tank.y > c.y))
                        {
                            scoreChild = -10000;

                            shoot = "UP#";
                            Console.WriteLine("Mytank-" + tank.y + "other y" + c.y + shoot);
                            break;
                        }
                        else if ((tank.y < c.y))
                        {
                            scoreChild = -10000;
                            shoot = "DOWN#";
                            Console.WriteLine("Mytank-" + tank.y + "other y" + c.y + shoot);
                            break;
                        }
                    }
                    if (tank.y == c.y)
                    {

                        if (((tank.x > c.x) && (tank.direction == 3)) || ((tank.x < c.x) && (tank.direction == 2)))
                        {
                            scoreChild = -10000;
                            shoot = "SHOOT#";
                            break;
                        }
                        else if ((tank.x > c.x))
                        {
                            scoreChild = -10000;
                            shoot = "LEFT#";
                            Console.WriteLine("Mytank-" + tank.x + "other x" + c.x + shoot);
                            break;
                        }
                        else if ((tank.x < c.x))
                        {
                            scoreChild = -10000;
                            shoot = "RIGHT#";
                            Console.WriteLine("Mytank-" + tank.x + "other x" + c.x + shoot);
                            break;
                        }
                    }
                }
            }

            if (grid.GetGrid()[node.x, node.y].GetType() == typeof(LifePack))
            {
                scoreChild += 1000;

            }
            else if (grid.GetGrid()[node.x, node.y].GetType() == typeof(Coin))
            {
                scoreChild += 750;

            }
            else if (grid.GetGrid()[node.x, node.y].GetType() == typeof(GameEntity))
            {
                scoreChild += 500;

            }
            else
            {
                scoreChild -= 2000;

            }



            Console.WriteLine("score child" + scoreChild);
            return scoreChild;

        }

    }
}