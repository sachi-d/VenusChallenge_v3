using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace VenusGame
{
    class GameGrid
    {
        public MyTank mytank;
        private GameEntity[,] gameGrid;
        public bool isGridSet;
        int[,] damgesLevel = new int[10, 10];
        List<string> bricks = new List<string>();
        List<string> stones = new List<string>();
        List<string> water = new List<string>();
        public List<Tank> tankList = new List<Tank>();

        Timer disTimer;
        int x;
        int y;
        bool isStarted=false;
        internal List<Tank> TankList
        {
            get { return tankList; }
            set { tankList = value; }
        }
        int u = 0;

        public int size = 10;

        //private int cellDistance;
        //private int upperBound;
        //private int leftBound;

        Game1 game;


        public GameGrid(Game1 g)
        {
            game = g;
            gameGrid = new GameEntity[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    gameGrid[i, j] = new GameEntity();
                }
            }
            mytank = new MyTank();
            mytank.setGrid(this);
            x = 0;
            y = 0;
            isGridSet = false;
            //     mynode = new Node();

        }
        //public int getCellDistance(){  return cellDistance; }
        //public int getUpperBound() { return upperBound; }
        //public int getLeftBound() { return leftBound; }
        //public void setCellDistance(int v) { cellDistance = v; }
        //public void setUpperBound(int v) { upperBound = v; }
        //public void setLeftBound(int v) { leftBound = v; }

        public GameEntity[,] GetGrid()
        {
            return this.gameGrid;
        }

        public bool getisStarted()
        {
            return isStarted;
        }
        public void setMapDetails(String m) //add received map data in to the game grid
        {
            isStarted = true;
            m = m.Remove(m.Length - 2);
            string[] mapDetails = new string[5];
            mapDetails = m.Split(':');

            mytank.setPlayerName(mapDetails[1].Substring(1));
            Console.WriteLine("MYTANKS NAME SAVED AS- " + mytank.getPlayerDigit());
            bricks.AddRange(mapDetails[2].Split(';'));
            stones.AddRange(mapDetails[3].Split(';'));
            water.AddRange(mapDetails[4].Split(';'));
            string[] bricks_array = bricks.ToArray();

            string[] stones_array = stones.ToArray();
            string[] water_array = water.ToArray();
            foreach (string s in bricks_array)
            {
                int q = Int32.Parse(s.Split(',')[0]);
                int w = Int32.Parse(s.Split(',')[1]);
                gameGrid[q, w] = new Brick(q, w);
            }
            foreach (string s in stones_array)
            {
                int q = Int32.Parse(s.Split(',')[0]);
                int w = Int32.Parse(s.Split(',')[1]);
                gameGrid[q, w] = new Stone(q, w);
            }
            foreach (string s in water_array)
            {
                int q = Int32.Parse(s.Split(',')[0]);
                int w = Int32.Parse(s.Split(',')[1]);
                gameGrid[q, w] = new Water(q, w);
            }

        }

        public void displayGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(gameGrid[j, i].ToString() + "  ");
                }
                Console.WriteLine("\n");
            }
        }



        public void setGlobalUpdate(string updatedValues) // once per second server will broadcast all the details about what happend in the gamegrid.
        {
            updatedValues = updatedValues.Remove(updatedValues.Length - 2);
            string[] c = updatedValues.Split(':');
            Console.Write(updatedValues);
            //Console.WriteLine("damage levs- " + c[c.Length - 1]);
            updateDamages(c[c.Length - 1]);
            // IDictionary<string, Tank> col = new Dictionary<string, Tank>();
            for (int i = 0; i < c.Length - 2; i++)
            {
                string[] cl = c[i + 1].Split(';');
                x = Int32.Parse(cl[1].ElementAt(0).ToString());
                y = Int32.Parse(cl[1].ElementAt(2).ToString());
                tankList[i].setPlayerDigit(i);
                tankList[i].setDirection(Int32.Parse(cl[2]));
                tankList[i].setGridLocation(x, y);
                tankList[i].setwhethershot(Int32.Parse(cl[3]));
                tankList[i].setHealth(Int32.Parse(cl[4]));
                tankList[i].setCoins(Int32.Parse(cl[5]));
                tankList[i].setPoints(Int32.Parse(cl[6]));
            }
            tankList.ToArray();
        }

        public void updateDamages(string command)
        {
            string[] dam = command.Split(';');
            for (int i = 0; i < dam.Length; i++)
            {
                int aa = Int32.Parse(dam[i].Split(',')[0]);
                int bb = Int32.Parse(dam[i].Split(',')[1]);
                int cc = Int32.Parse(dam[i].Split(',')[2]);
                if (this.gameGrid[aa, bb].ToString().Equals("BB"))
                {
                    Brick b = (Brick)this.gameGrid[aa, bb];
                    b.setHealth(cc);
                }
                //if (cc == 4)
                //{
                //    this.gameGrid[aa, bb] = new GameEntity();
                //}
            }
        }
        public void getCoinsDetails(string coinmessage)
        {
            coinmessage = coinmessage.Remove(coinmessage.Length - 2);
            string[] coinDetails = new string[4];
            //Console.ReadLine();
            coinDetails = coinmessage.Split(':');
            int coin_x = Int32.Parse(coinDetails[1].Split(',')[0]);
            int coin_y = Int32.Parse(coinDetails[1].Split(',')[1]);
            int LT = Int32.Parse(coinDetails[2]);
            int val = Int32.Parse(coinDetails[3]);
            Coin c = new Coin(coin_x, coin_y, LT, val);
            processLifeTime(c);
            this.gameGrid[coin_x, coin_y] = c;

            // the code that you want to measure comes here


        }
        public void getLifePacksDetails(string lpmessage)
        {
            lpmessage = lpmessage.Remove(lpmessage.Length - 2);
            string[] lpDetails = new string[3];

            lpDetails = lpmessage.Split(':');
            int lp_x = Int32.Parse(lpDetails[1].Split(',')[0]);
            int lp_y = Int32.Parse(lpDetails[1].Split(',')[1]);
            int LT = Int32.Parse(lpDetails[2]);
            LifePack l = new LifePack(lp_x, lp_y, LT);
            processLifeTime(l);
            //this.gameGrid[lp_x, lp_y] = l;
        }
        public void processLifeTime(LifePack en)
        {
            this.gameGrid[en.getX(), en.getY()] = en;

            disTimer = new System.Timers.Timer();
            disTimer.Elapsed += (source, e) => OnTimedEvent(source, e, en);

            disTimer.Interval = en.getLifeTime();

            disTimer.Enabled = true;
           // timer.Elapsed = new ElapsedEventHandler();
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e, LifePack p)
        {
            this.gameGrid[p.x, p.y] = new GameEntity();

            disTimer.Enabled = false;
        }
        public void setLocation(string l) // Go to the initial setup location.
        {

            l = l.Remove(l.Length - 2); //remove trailing # and ?
            string[] message = new string[6];
            message = l.Split(':');
            for (int c = 1; c < message.Length; c++)
            {
                Tank ttt;
                if (c == mytank.getPlayerDigit() + 1)
                {
                    ttt = mytank;
                    Console.WriteLine("mytank---------------------");
                }
                else
                {
                    ttt = new Tank();
                    ttt.setPlayerName(message[1].ElementAt(1).ToString());
                    ttt.setGrid(this);
                    Console.WriteLine("new tank---------------------");
                }
                string[] myplayer = new string[4];
                myplayer = message[ttt.getPlayerDigit() + 1].Split(';', ',');
                x = Int32.Parse(myplayer[1]);
                y = Int32.Parse(myplayer[2]);
                ttt.setDirection(Int32.Parse(myplayer[3]));
                gameGrid[x, y] = ttt;
                tankList.Add(ttt);
            }

        }
        public String readServerMessage(string message)
        {
            String res = "";
            if (message[0] == 'S')
            {
                //Console.Write("Joined the game\n");
                //Console.WriteLine("---" + message + "---");
                setLocation(message);
            }
            else if (message[0] == 'I')
            {
                //Console.Write("Game initialised\n");
                setMapDetails(message);
                isGridSet = true;

            }
            else if (message[0] == 'G')
            {
                // Console.Write("Global update\n");
                setGlobalUpdate(message);
            }
            else if (message[0] == 'C')
            {
                //Console.Write("coins!!\n");
                getCoinsDetails(message);
            }
            else if (message[0] == 'L')
            {
                //Console.Write("life packs!! \n");
                getLifePacksDetails(message);
            }
            else
            {
                res=respondCommands(message);
            }
            //Console.WriteLine("----------------" + message + "-----------------");
            this.displayGrid();
            return res;
        }

        public string respondCommands(String x)
        {
            String respond;
            x = x.Split('#')[0];
            if (x == "OBSTACLE")
            {
                respond = "Obstacle found!";
                Console.WriteLine(respond);
                return respond;
            }
            else if (x == "CELL_OCCUPIED")
            {
                respond = "Occupied cell!";
                Console.WriteLine(respond);
                return respond;
            }
            else if (x == "DEAD")
            {
                respond = "Player dead!";
                Console.WriteLine(respond);
                return respond;
            }
            else if (x == "TOO_QUICK")
            {
                respond = "WHOA! Slow down!";
                Console.WriteLine(respond);
                return respond;
            }
            else if (x == "INVALID_CELL")
            {
                respond = "Not a valid cell";
                Console.WriteLine(respond);
                return respond;
            }
            else if (x == "GAME_HAS_FINISHED")
            {
                respond = "Game end";
                Console.WriteLine(respond);
                return respond;
            }
            else if (x == "PITFALL")
            {
                respond = "Pitfall - Game end";
                Console.WriteLine(respond);
                return respond;
            }
            else if (x == "GAME_NOT_STARTED_YET")
            {
                respond = "Game will start soon";
                Console.WriteLine(respond);
                return respond;
            }
            else if (x == "NOT_A_VALID_CONTESTANT")
            {
                respond = "Not a valid contestant";
                Console.WriteLine(respond);
                return respond;
            }
            else
            {
                respond = "";
                // Console.WriteLine("Not a valid respond");
                return respond;

            }
        }
    }



}
