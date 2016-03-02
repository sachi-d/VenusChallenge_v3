using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace VenusGame
{
    class Init
    {
        Client2 cli;
        Game1 game;
        AI ai;
        public Init()
        {
            //game = new Game1();
            Thread gui = new Thread(new ThreadStart(StartGUI));
            gui.Start();

            Thread form = new Thread(new ThreadStart(StartForm));
            form.Start();

            Thread.Sleep(1000);
            cli.SetGame(game);


            //Thread aiNew = new Thread(new ThreadStart(startAI));
            //gui.Start();
        }

        private void StartGUI()
        {
            game = new Game1();
            using (game)
            {
                while (!game.Gamegrid.getisStarted())
                {

                }
                game.Run();
                
            }
        }

        public void StartForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            cli = new Client2();
            Application.Run(cli);
        }
    }
}
