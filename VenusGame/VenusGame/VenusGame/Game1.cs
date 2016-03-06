using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace VenusGame
{
    public struct CellData
    {
        public Vector2 position;
        public int health;
        public string type;
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;

        Texture2D backgroundTexture;
        Texture2D cellTexture;
        Texture2D brickTexture;     //  0 damage
        Texture2D brick1Texture;    //  25% damage
        Texture2D brick2Texture;    //  50%damage
        Texture2D brick3Texture;    //  75% damage
        Texture2D stoneTexture;
        Texture2D waterTexture;
        Texture2D foregroundTexture;
        Texture2D tankTexture;
        Texture2D lifepackTexture;
        Texture2D coinTexture;
        Texture2D op0Texture;
        Texture2D op1Texture;
        Texture2D op2Texture;
        Texture2D op3Texture;
        Texture2D op4Texture;
        Texture2D bulletTexture;
        SpriteFont font1;
        private Texture2D container, lifebar;
        private Vector2 barposition;
        int fullhealth;
        int currenthealth;
        int coins;
        int points;

        int screenWidth = 1200; //FIXED BACKGROUND WIDTH
        int screenHeight = 675;   //FIXED BACKGROUNDHEIGHT
        int cellsize = 60;      //FIXED CELL WIDTH

        int topBoundary;
        int leftBoundary;

        GameGrid gamegrid;
        Vector2 rotationpoint = new Vector2(30, 30);
        String displayText = " ";
       
        internal GameGrid Gamegrid
        {
            get { return gamegrid; }
            set { gamegrid = value; }
        }
        int cellcount;

        int upcount = 0;
        


        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gamegrid = new GameGrid(this);
            cellcount = gamegrid.size;
            topBoundary = (screenHeight - (cellcount * cellsize)) / 2;
            leftBoundary = (screenWidth - (cellcount * cellsize)) / 2;
            
            //clientconnection = cli;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Venus Challenge - Tank Game";
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;
            backgroundTexture = Content.Load<Texture2D>("background");
            foregroundTexture = Content.Load<Texture2D>("foreground");
            cellTexture = Content.Load<Texture2D>("cell");
            brickTexture = Content.Load<Texture2D>("brick");
            brick1Texture = Content.Load<Texture2D>("brick1");
            brick2Texture = Content.Load<Texture2D>("brick2");
            brick3Texture = Content.Load<Texture2D>("brick3");
            stoneTexture = Content.Load<Texture2D>("stone");
            waterTexture = Content.Load<Texture2D>("water");
            tankTexture = Content.Load<Texture2D>("tank");
            lifepackTexture = Content.Load<Texture2D>("lifepack");
            coinTexture = Content.Load<Texture2D>("coin");
            op0Texture = Content.Load<Texture2D>("op0");
            op1Texture = Content.Load<Texture2D>("op1");
            op2Texture = Content.Load<Texture2D>("op2");
            op3Texture = Content.Load<Texture2D>("op3");
            op4Texture = Content.Load<Texture2D>("op4");
            bulletTexture = Content.Load<Texture2D>("bullet");
            font1 = Content.Load<SpriteFont>("Myfont");
            container = Content.Load<Texture2D>("healthbar");
            lifebar = Content.Load<Texture2D>("healthmanage");

            barposition = new Vector2(920, 30);
            fullhealth = 205;
            coins = 0;
            points = 0;
            //gamegrid.mytank.angle = 0;
            //screenWidth = device.PresentationParameters.BackBufferWidth;
            //screenHeight = device.PresentationParameters.BackBufferHeight;
            //SetUpGrid();


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Updatehealthbar();
           
            if (upcount == 30)
            {
                upcount = 0;
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    this.Exit();


                base.Update(gameTime);

            }
            else
            {
                upcount++;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            DrawScenery();
            DrawHealthbar();
            DrawCells();
            DrawText();
            
            //DrawBullets();
            //DrawBars();


            spriteBatch.End();

            base.Draw(gameTime);
        }
        private void Updatehealthbar()
        {
            int heal = gamegrid.mytank.getHealth();
            coins = gamegrid.mytank.getCoins();
            points = gamegrid.mytank.getPoints();
            currenthealth = fullhealth * heal / 100;
        }

        private void DrawHealthbar()
        {
            
            //Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(container, barposition, Color.White);
            spriteBatch.Draw(lifebar, new Rectangle((int)barposition.X+18+currenthealth, (int)barposition.Y+25, fullhealth-currenthealth, 33), Color.White);
            
        }
        private void DrawTank(Tank t, Texture2D tex)
        {
            Vector2 position = new Vector2(t.x * cellsize + leftBoundary + cellsize / 2, t.y * cellsize + topBoundary + cellsize / 2);
            //Vector2 rotationpoint = new Vector2(position.X + 30, position.Y + 30);

            spriteBatch.Draw(cellTexture, position, null, Color.White, 0, rotationpoint, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(tex, position, null, Color.White, t.angle, rotationpoint, 1, SpriteEffects.None, 1);

        }
        private void DrawText()
        {
            spriteBatch.DrawString(font1, displayText, new Vector2(29, 595), Color.Black);
            spriteBatch.DrawString(font1, "Coins : "+coins.ToString(), new Vector2(barposition.X+70, barposition.Y+95), Color.Black);
            spriteBatch.DrawString(font1, "Points: " + points.ToString(), new Vector2(barposition.X+70, barposition.Y + 125), Color.Black);
            //spriteBatch.DrawString(font, "Cannon power: 100", new Vector2(20, 45), Color.White);
        }
        public void readMessage(String m)
        {
            displayText = gamegrid.readServerMessage(m);
        }

        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            spriteBatch.Draw(foregroundTexture, screenRectangle, Color.White);
        }

        private void DrawBars()
        {
            //drAW health n coins display
        }
        private void DrawCells()
        {
            Texture2D tex = cellTexture;
            for (int i = 0; i < gamegrid.size; i++)
            {
                for (int j = 0; j < gamegrid.size; j++)
                {
                    GameEntity m = gamegrid.GetGrid()[i, j];
                    m.pos = new Vector2(i, j);
                    Vector2 mpos = new Microsoft.Xna.Framework.Vector2(leftBoundary + i * cellsize, topBoundary + j * cellsize);
                    //Console.WriteLine("iiiiiiiiiiiiiiiiiiiiii" + m.ToString());

                    string field = m.ToString().Substring(0, 2);
                    switch (field)
                    {
                        case "BB":
                            Brick b = (Brick)m;
                            int hel = b.getHealth();
                            switch (hel)
                            {
                                case 1:
                                    tex = brick1Texture;
                                    break;
                                case 2:
                                    tex = brick2Texture;
                                    break;
                                case 3:
                                    tex = brick3Texture;
                                    break;
                                case 4:
                                    tex = cellTexture;
                                    break;
                                default:
                                    tex = brickTexture;
                                    break;
                            }
                            break;
                        case "SS":
                            tex = stoneTexture;
                            break;
                        case "WW":
                            tex = waterTexture;
                            break;
                        case "CC":
                            tex = coinTexture;
                            break;
                        case "LP":
                            tex = lifepackTexture;
                            break;
                        case "PP":
                            string subfield = m.ToString().Substring(2, 1);
                            switch (subfield)
                            {
                                case "M":
                                    tex = tankTexture;
                                    DrawTank(gamegrid.mytank, tex);
                                    break;
                                case "0":
                                    tex = op0Texture;
                                    DrawTank(gamegrid.TankList[0], tex);
                                    break;
                                case "1":
                                    tex = op1Texture;
                                    DrawTank(gamegrid.TankList[1], tex);
                                    break;
                                case "2":
                                    tex = op2Texture;
                                    DrawTank(gamegrid.TankList[2], tex);
                                    break;
                                case "3":
                                    tex = op3Texture;
                                    DrawTank(gamegrid.TankList[3], tex);
                                    break;
                                case "4":
                                    tex = op4Texture;
                                    DrawTank(gamegrid.TankList[4], tex);
                                    break;
                            }
                            continue;
                        default:
                            tex = cellTexture;
                            break;

                    }
                    //if (field.Equals("BB"))
                    //{
                    //    Brick b = (Brick)m;
                    //    if (b.getHealth() == 1)
                    //    {
                    //        tex = brick1Texture;
                    //    }
                    //    else if (b.getHealth() == 2)
                    //    {
                    //        tex = brick2Texture;
                    //    }
                    //    else if (b.getHealth() == 3)
                    //    {
                    //        tex = brick3Texture;
                    //    }
                    //    else if (b.getHealth() == 4)
                    //    {
                    //        tex = cellTexture;
                    //    }
                    //    else
                    //    {
                    //        tex = brickTexture;
                    //    }

                    //}
                    //else if (field.Equals("SS"))
                    //{
                    //    tex = stoneTexture;
                    //}
                    //else if (field.Equals("WW"))
                    //{
                    //    tex = waterTexture;
                    //}
                    //else if (field.Equals("CC"))
                    //{
                    //    tex = coinTexture;
                    //}
                    //else if (field.Equals("LP"))
                    //{
                    //    tex = lifepackTexture;
                    //}
                    //else if (field.Equals("PP"))
                    //{
                    //    string subfield = m.ToString().Substring(2, 1);
                    //    if (subfield.Equals("M"))
                    //    {
                    //        tex = tankTexture;
                    //        DrawTank(gamegrid.mytank, tex);
                    //    }
                    //    else if (subfield.Equals("0"))
                    //    {
                    //        tex = op0Texture;
                    //        DrawTank(gamegrid.TankList[0], tex);
                    //    }
                    //    else if (subfield.Equals("1"))
                    //    {
                    //        tex = op1Texture;
                    //        DrawTank(gamegrid.TankList[1], tex);
                    //    }
                    //    else if (subfield.Equals("2"))
                    //    {
                    //        tex = op2Texture;
                    //        DrawTank(gamegrid.TankList[2], tex);
                    //    }
                    //    else if (subfield.Equals("3"))
                    //    {
                    //        tex = op3Texture;
                    //        DrawTank(gamegrid.TankList[3], tex);
                    //    }
                    //    else if (subfield.Equals("4"))
                    //    {
                    //        tex = op4Texture;
                    //        DrawTank(gamegrid.TankList[4], tex);
                    //    }
                    //    Console.WriteLine("Subfield---------------------" + subfield+" m======"+m);
                    //    continue;
                    //}
                    //else
                    //{
                    //    tex = cellTexture;
                    //}
                    spriteBatch.Draw(tex, mpos, Color.White);
                }
            }

        }

        public void DrawBullets()
        {
            if (gamegrid.mytank.getShooting())
            {
                gamegrid.mytank.bulletpos += gamegrid.mytank.dirpos;
                spriteBatch.Draw(bulletTexture, gamegrid.mytank.bulletpos, null, Color.White, gamegrid.mytank.angle, rotationpoint, 1, SpriteEffects.None, 1);
                //WAIT FOR 1/3 SECONDS
            }
            if (gamegrid.mytank.bulletpos == (gamegrid.mytank.pos + (gamegrid.mytank.dirpos * gamegrid.mytank.getShootLength())))
            {
                gamegrid.mytank.bulletpos = gamegrid.mytank.pos;
                setShooting(false);
            }
        }

        public void setShooting(bool m)
        {
            gamegrid.mytank.bulletpos = gamegrid.mytank.pos;
            gamegrid.mytank.setShooting(m);
        }

    }

}
