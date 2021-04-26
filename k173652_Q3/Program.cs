using k173652_Q1;
using k173652_Q1_;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k173652_Q3
{
    class Program
    {
        static void MoveFolders(string fromDir, string toDir)
        {
            Move(getRecentDirectory(fromDir));
           
            void Move(string recentDir)
            {
                foreach (var dir in Directory.GetDirectories(fromDir))
                {
                    if (recentDir != dir)
                    {
                        Directory.Move(dir, toDir + @"\" + dir.Replace(fromDir, ""));

                    }
                }
            }
            string getRecentDirectory(string currentDir)
            {
                return (from f in new DirectoryInfo(currentDir).
                       GetDirectories()
                        orderby f.LastWriteTimeUtc descending
                        select f).First().FullName;
            }
        }
        static void Main(string[] args)
        {

           
            ProcessGenerator.waitTime = Convert.ToInt32(ConfigurationManager.AppSettings["setTimeInMinutes"]);
            ProcessGenerator.MyAction = () => MoveFolders(ConfigurationManager.AppSettings["FromDir"], ConfigurationManager.AppSettings["ToDir"]);
            Environment.ExitCode = WindowService.RunWindowsService("Q3 Data Moving", "Q1 Data Moving", "Q1 Data Moving with a timer period " + ProcessGenerator.waitTime + " min", "Q3");

        }
    }
}
