using k173652_Q1;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Topshelf;

namespace k173652_Q1_
{
    class Program
    {
        class InternetUnvailable : Exception
        {
            
            public InternetUnvailable(): base(String.Format("Internet is not available"))
            {

            }

        }
        private static void RunProcess(string directory)
        {
            try
            {
                if (new Ping().Send("www.google.com.mx").Status == IPStatus.Success)
                {
                    using (Process myProcess = new Process())
                    {
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.FileName = directory;
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.Start();
                    }

                }
            }
            catch(Exception e)
            {
                throw new InternetUnvailable();
            }
            


        }
          
        static void Main()
        {
            //RunProcess(ConfigurationManager.AppSettings["Question1Directive"]);
        
           ProcessGenerator.waitTime = Convert.ToInt32(ConfigurationManager.AppSettings["setTimeInMinutes"]);
            ProcessGenerator.MyAction=() => RunProcess(ConfigurationManager.AppSettings["Question1Directive"]);
          Environment.ExitCode= WindowService.RunWindowsService("Q1 Data Fetching", "Q1 Data Fetching", "Q1 Data Fetching with a timer period " + ProcessGenerator.waitTime+" min","Q1");


        }
    }
}
