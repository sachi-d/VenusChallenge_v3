using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Drawing;
using System.Collections;

namespace MyTankGame
{
    public partial class Client : Form
    {
        GameGrid grid = null;
        Tank myTank = new Tank();
        private NetworkStream clientStream; //Stream - outgoing
        private TcpClient client; //To talk back to the client
        private BinaryWriter writer; //To write to the clients

        private NetworkStream serverStream; //Stream - incoming        
        private TcpListener listener; //To listen to the clinets        
        public string reply = ""; //The message to be written

        int serverPort = 6000;
        int clientPort = 7000;
        Thread receive_thread;
        public Client()
        {
            InitializeComponent();
            grid = new GameGrid();
        }

        private void Join_btn_Click(object sender, EventArgs e)
        {
            SendData("JOIN#");
            startRecieve();
            Join_Btn.Enabled = false;
        }

        private void Send_btn_Click(object sender, EventArgs e)
        {
            String command = textBox1.Text;
            SendData(command);
        }

        public void startRecieve()
        {
            receive_thread = new Thread(new ThreadStart(ReceiveData));
            receive_thread.Start();
        }
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
                            port = Convert.ToInt32(ss);
                        }
                        catch (Exception)
                        {
                            port = 100;
                        }

                        //Console.WriteLine(ip + ": " + reply);
                        //   dataObj = new DataObject(reply.Substring(0, reply.Length - 1), ip, port);
                        //String message = reply.Substring(0, reply.Length - 1);
                        // ThreadPool.QueueUserWorkItem(new WaitCallback(Program.Resolve),message);
                        grid.readServerMessage(reply);

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
            Console.WriteLine("check");
            if (keyData == Keys.Left)
            {
                SendData("LEFT#");
                return true;
            }
            else if (keyData == Keys.Right)
            {
                SendData("RIGHT#");
                return true;
            }
            else if (keyData == Keys.Up)
            {
                SendData("UP#");
                return true;
            }
            else if (keyData == Keys.Down)
            {
                SendData("DOWN#");
                return true;
            }
            else if (keyData == Keys.Space)
            {
                SendData("SHOOT#");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
