using k173652_Q1;
using System;
using System.Configuration;
using System.Diagnostics;
using Topshelf;

namespace k173652_Q1_
{
    class Program
    {
        private static void RunProcess(string directory)
        {
            
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = directory;
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                }
            
            


        }
        static void Main()
        {
          
           // ProcessGenerator.directory = ConfigurationManager.AppSettings["Question1Directive"];
            ProcessGenerator.waitTime = Convert.ToInt32(ConfigurationManager.AppSettings["setTimeInMinutes"]);
            ProcessGenerator.MyAction=() => RunProcess(ConfigurationManager.AppSettings["Question1Directive"]);
          Environment.ExitCode= WindowService.RunWindowsService("Q1 Data Fetching", "Q1 Data Fetching", "Q1 Data Fetching with a timer period " + ProcessGenerator.waitTime+" min","Q1");


        }
    }
}
