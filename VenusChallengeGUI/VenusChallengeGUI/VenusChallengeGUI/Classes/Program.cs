using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;


namespace MyTankGame
{
    class Program
    {
        //public void WorkThreadFunction(Client_2 communicator)
        //{
        //    try
        //    {

        //        communicator.ReceiveData();
        //    }
        //    catch (Exception ex)
        //    {
        //        // log errors
        //    }
        //}
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Client());

        }
    }
}
