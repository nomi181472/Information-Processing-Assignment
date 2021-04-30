using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace k173652_Q1_
{
   public class ProcessGenerator
    {

        private readonly Timer timer;
        public string question { get; set; }
        static public int waitTime;
        static public Action MyAction { get; set; }
        public ProcessGenerator()
        {
            this.timer = new Timer((waitTime*1000)*60)
            {
                AutoReset = true
            };
            this.timer.Elapsed += this.RunMyAction;
        }
       

        private void RunMyAction(object sender, ElapsedEventArgs e)
        {
            try
            {
                MyAction();
                var msg =new string[] { "Question: " + question + " Time: " + DateTime.Now.ToString() };
                File.AppendAllLines(@"D:\Assignment\SuccessReport.txt", msg);
            }
            catch(Exception Ex)
            {
                var msg= new string[] { "Question: " + question + " Reason: " + Ex.Message + " Time: " + DateTime.Now.ToString() };
                File.AppendAllLines(@"D:\Assignment\ExceptionReport.txt", msg);
            }

        }
        
        public void Start(string question)
        {
            this.question = question;
            this.timer.Start();

        }
        public void Stop()
        {

            this.timer.Stop();
        }

    }
}
