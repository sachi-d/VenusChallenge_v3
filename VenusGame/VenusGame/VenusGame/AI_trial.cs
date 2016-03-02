using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace VenusGame
{
    class AI_trial
    {
        public bool gameOver = false;
        Client2 client;
        private Timer disTimer;
        Timer start;
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
        String previous;
        String h;
        int firstTime;
        Boolean finish;
        public AI_trial(Game1 g, Client2 cli)
        {

            //this._pingTimer = new Timer();
            //this._pingTimer.Interval = 200;
            //this._pingTimer.Elapsed += new ElapsedEventHandler(this.TimeElapsed);

            ////trial run
            ////   this._pingTimer.Elapsed += new ElapsedEventHandler(this.TrialRun);


            //this._pingTimer.Enabled = true;
            start = new Timer();
            start.Elapsed += start_Elapsed;
            start.Interval = 10000;
            start.Enabled = true;

            

            this.client = cli;

            setGrid(g.Gamegrid);

            count = 0;
            score = 0;
            county = true;
            finish = false;
            firstTime = 1;
            cell = new GameEntity();
            Random rnd = new Random();
        }

        void start_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Wait for 10 seconds" );
            startsend();
            start.Enabled = false;
        }

        void disTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Timer - " + count);
            count++;
            client.SendData("RIGHT#");
        }
        public void setGrid(GameGrid g)
        {
            this.grid = g;
        }

        public void startsend()
        {
            disTimer = new System.Timers.Timer();
            disTimer.Elapsed += disTimer_Elapsed;

            disTimer.Interval = 990;

            disTimer.Enabled = true;

        }
        public void TimeElapsed(Object sender, ElapsedEventArgs eventArgs)
        {
        }
    }
}
