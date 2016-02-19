using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MyTankGame
{
    class GameGrid
    {
        public Tank mytank;
        public GameEntity[,] gameGrid;
        int brickCount = 0;
        int[,] damgesLevel = new int[10, 10];
        List<string> bricks = new List<string>();
        List<string> stones = new List<string>();
        List<string> water = new List<string>();
        List<Tank> tankList = new List<Tank>();
        
        int x;
        int y;
        

        public GameGrid()
        {
            gameGrid = new GameEntity[10, 10];
            mytank = new Tank();
            mytank.setGrid(this);
            x = 0;
            y = 0;
          

        }
        public void setTank(int x,int y){
            gameGrid[x, y] = mytank;
        }
        public void setMapDetails(String m) //add received map data in to the game grid
        {
            m = m.Remove(m.Length - 2);
            string[] mapDetails = new string[5];
            mapDetails = m.Split(':');
            
            mytank.setPlayerName(mapDetails[1].Substring(1));
            
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
                gameGrid[w, q] = new Brick(q, w);
            }
            foreach (string s in stones_array)
            {
                int q = Int32.Parse(s.Split(',')[0]);
                int w = Int32.Parse(s.Split(',')[1]);
                gameGrid[w, q] = new Stone(q, w);
            }
            foreach (string s in water_array)
            {
                int q = Int32.Parse(s.Split(',')[0]);
                int w = Int32.Parse(s.Split(',')[1]);
                gameGrid[w,q] = new Water(q, w);
            }

        }

        public void displayGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (gameGrid[i, j] != null)
                    {
                        Console.Write(gameGrid[i, j].ToString() + " ");
                    }
                    else
                    {
                        Console.Write("-- ");
                    }
                }
                Console.WriteLine("\n");
            }
        }

        public void setGlobalUpdate(string updatedValues) // once per second server will broadcast all the details about what happend in the gamegrid.
        {
            
            updatedValues = updatedValues.Remove(updatedValues.Length - 2);
            string[] c = updatedValues.Split(':');
            Console.Write(updatedValues);
            int p = mytank.prevX;
            int q = mytank.prevY;
            Console.WriteLine("p" + p);
            Console.WriteLine("p" + q);
            
           // updateDamages(c[c.Length - 1]);
            IDictionary<string, Tank> col = new Dictionary<string, Tank>();
            for (int i = 0; i < c.Length - 2; i++)
            {
               
                
                if (i == Int32.Parse(mytank.playerName))
                {
                    
                    mytank.globalUpdate(c[i+1]);
                    Console.WriteLine(c[i+1]);
                    
                    
                    if ((p == x && q == y) || (x==null))
                    {
                     
                    }
                    else
                    {
                        
                        mytank.prevX = x;
                        mytank.prevY = y;

                        this.gameGrid[p, q] = null;
                    }
                    string[] cl = c[i + 1].Split(';');
                     y = Int32.Parse(cl[1].ElementAt(0).ToString());
                     x = Int32.Parse(cl[1].ElementAt(2).ToString());
                
                    this.gameGrid[x,y] = mytank;
                }
                else
                {
                    
                    string[] cl = c[i + 1].Split(';');
                   
                        tankList.Add(new Tank(cl[0].ElementAt(1).ToString()));
                    
                    y = Int32.Parse(cl[1].ElementAt(0).ToString());
                    x = Int32.Parse(cl[1].ElementAt(2).ToString());
                    tankList.Last().globalUpdate(c[i + 1]);
                    this.gameGrid[x, y] = tankList.Last();




                   
                }
                
                Console.WriteLine("preX  " + mytank.prevX);
                Console.WriteLine("preY  " + mytank.prevY);
                Console.WriteLine("X  " + x);
                Console.WriteLine("y  " + y);

            }
            tankList.ToArray();


        }

        public void updateDamages(string command)
        {
            Console.Write("blah blah um updating damages.....");
            List<string> damages = new List<string>();
            damages.AddRange(command.Split(';'));
            foreach (string s in damages)
            {
                int c = Int32.Parse(s.Split(';')[0]);
                int d = Int32.Parse(s.Split(';')[1]);
                damgesLevel[c, d] = Int32.Parse(s.Split(';')[2]);
            }
        }
        public void getCoinsDetails(string coinmessage)
        {
            coinmessage = coinmessage.Remove(coinmessage.Length - 2);
            string[] coinDetails = new string[4];
            Console.WriteLine("coins" + coinmessage);
            //Console.ReadLine();
            coinDetails = coinmessage.Split(':');
            int coin_y = Int32.Parse(coinDetails[1].Split(',')[0]);
            int coin_x = Int32.Parse(coinDetails[1].Split(',')[1]);
            int LT = Int32.Parse(coinDetails[2]);
            int val = Int32.Parse(coinDetails[3]);
            Coin c = new Coin(coin_x, coin_y, LT, val);
         
            this.gameGrid[coin_x, coin_y] = new Coin(coin_y, coin_x, LT, val);
           
            // the code that you want to measure comes here
           
            
        }
        public void getLifePacksDetails(string lpmessage)
        {
            lpmessage = lpmessage.Remove(lpmessage.Length - 2);
            string[] lpDetails = new string[3];

            lpDetails = lpmessage.Split(':');
            int lp_y = Int32.Parse(lpDetails[1].Split(',')[0]);
            int lp_x= Int32.Parse(lpDetails[1].Split(',')[1]);
            int LT = Int32.Parse(lpDetails[2]);
            this.gameGrid[lp_x, lp_y] = new LifePack(lp_y, lp_x, LT);
        }

        public void readServerMessage(string message)
        {

            string s = "";
            if (message[0] == 'S')
            {
                Console.Write("Joined the game\n");
                s = "Joined the game\n";
                //Console.WriteLine("---" + message + "---");
                mytank.setLocation(message);
            }
            else if (message[0] == 'I')
            {
                Console.Write("Game initialised\n");
                s = "Game initialised\n";
                setMapDetails(message);
            }
            else if (message[0] == 'G')
            {
                // Console.Write("Global update\n");
                s = "Global update\n";
                setGlobalUpdate(message);
            }
            else if (message[0] == 'C')
            {
                Console.Write("coins!!\n");
                s = "coins\n";
                getCoinsDetails(message);
            }
            else if (message[0] == 'L')
            {
                Console.Write("life packs!! \n");
                s = "life packs!";
                getLifePacksDetails(message);
            }
            this.displayGrid();

        }
       
    }

}
