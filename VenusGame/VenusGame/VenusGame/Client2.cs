using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace VenusGame
{
    public partial class Client2 : Form
    {
        private NetworkStream clientStream; //Stream - outgoing
        private TcpClient client; //To talk back to the client
        private BinaryWriter writer; //To write to the clients

        private NetworkStream serverStream; //Stream - incoming        
        private TcpListener listener; //To listen to the clinets        
        public string reply = ""; //The message to be written

        int serverPort = 6000;
        int clientPort = 7000;
        Thread receive_thread;
        Thread send_thread;
        Game1 game;
        AI aiNew;
        AI_trial trialAI;
        bool isAIMode;
        //bool started;
        public Client2()
        {
            InitializeComponent();
            isAIMode = false;

        }
        public void SetGame(Game1 g)
        {
            game = g;
        }
     
        public void startRecieve()
        {
            receive_thread = new Thread(new ThreadStart(ReceiveData));
            receive_thread.Start();
        }
        //public void startSend(String x)
        //{
        //    send_thread = new Thread(()=>SendData(x)) ;
        //    send_thread.Start();
        //}
        public void ReceiveData()
        {

            bool errorOcurred = false;
            Socket connection = null; //The socket that is listened to       
            try
            {
                //Creating listening Socket
                this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"), clientPort);
                //Starts listening
                this.listener.Start();
                //Establish connection upon client request
                //      DataObject dataObj;
                while (true)
                {
                    //connection is connected socket
                    connection = listener.AcceptSocket();
                    if (connection.Connected)
                    {
                        //To read from socket create NetworkStream object associated with socket
                        this.serverStream = new NetworkStream(connection);

                        SocketAddress sockAdd = connection.RemoteEndPoint.Serialize();
                        string s = connection.RemoteEndPoint.ToString();
                        List<Byte> inputStr = new List<byte>();

                        int asw = 0;
                        while (asw != -1)
                        {
                            asw = this.serverStream.ReadByte();
                            inputStr.Add((Byte)asw);
                        }

                        reply = Encoding.UTF8.GetString(inputStr.ToArray());
                        //myTank.respondCommands(reply);
                        this.serverStream.Close();
                        string ip = s.Substring(0, s.IndexOf(":"));
                        int port = 100;
                        try
                        {
                            string ss = reply.Substring(0, reply.IndexOf(";"));
                            //Console.WriteLine(ss+" dsdsdsdsdsds");
                            //port = Convert.ToInt32(ss);
                        }
                        catch (Exception)
                        {
                            port = 100;
                        }

                        //Console.WriteLine(ip + ": " + reply);
                        //   dataObj = new DataObject(reply.Substring(0, reply.Length - 1), ip, port);
                        //String message = reply.Substring(0, reply.Length - 1);
                        // ThreadPool.QueueUserWorkItem(new WaitCallback(Program.Resolve),message);
                        //grid.readServerMessage(reply);

                        game.readMessage(reply);

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Communication (RECEIVING) Failed! \n " + e.Message);
                errorOcurred = true;
            }
            finally
            {
                if (connection != null)
                    if (connection.Connected)
                        connection.Close();
                if (errorOcurred)
                    this.ReceiveData();
            }
        }

        public void SendData(string x)
        {
            //DataObject dataObj = (DataObject)stateInfo;
            //Opening the connection
            this.client = new TcpClient();

            try
            {


                this.client.Connect(IPAddress.Parse("127.0.0.1"), serverPort);

                if (this.client.Connected)
                {
                    //To write to the socket
                    this.clientStream = client.GetStream();

                    //Create objects for writing across stream
                    this.writer = new BinaryWriter(clientStream);
                    Byte[] tempStr = Encoding.ASCII.GetBytes(x);

                    //writing to the port                
                    this.writer.Write(tempStr);
                    Console.WriteLine("\t Data: " + x + " is written to " + IPAddress.Parse("127.0.0.1") + " on " + serverPort);
                    this.writer.Close();

                    this.clientStream.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Communication (WRITING) to " + IPAddress.Parse("127.0.0.1") + " on " + serverPort + "Failed! \n " + e.Message);
            }
            finally
            {
                this.client.Close();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            string x = "empty";
            Console.WriteLine("check");
            if (keyData == Keys.Left)
            {
                x = "LEFT#";
            }
            else if (keyData == Keys.Right)
            {
                x = "RIGHT#";
            }
            else if (keyData == Keys.Up)
            {
                x = "UP#";
            }
            else if (keyData == Keys.Down)
            {
                x = "DOWN#";
            }
            else if (keyData == Keys.Space)
            {
                x = "SHOOT#";
                game.setShooting(true);
                game.DrawBullets();
            }

            if (!x.Equals("null"))
            {
                SendData(x);
                Console.WriteLine("zzzzzzzzzzzzzzzz---- " + game);
                //game.Communicate(x);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendData("JOIN#");
            startRecieve();
            button1.Enabled = false;
            if (isAIMode)
            {
                trialAI = new AI_trial(game, this);
                //aiNew = new AI(game, this);
                checkBox1.Enabled = false;
                this.Visible = false;
            }
            else
            {
                this.ClientSize = new System.Drawing.Size(100, 30);
                this.TopMost = true;
                //this.position
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (isAIMode)
            {
                isAIMode = false;
            }
            else
            {
                isAIMode = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
          //  Process.Start("http://venuschallenge.esy.es/");
            System.Diagnostics.Process.Start("http://venuschallenge.esy.es/");
        }

      

    }
}
