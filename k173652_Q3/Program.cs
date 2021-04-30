using k173652_Q1;
using k173652_Q1_;
using k173652_Q2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace k173652_Q3
{
    class Program
    {
        static void MoveFolders(string fromDir, string toDir,string reportPath)
        {
            Move(getRecentDirectory(fromDir));

            void Move(string recentDir)
            {
                var directories = Directory.GetDirectories(fromDir);
                foreach (var dir in directories)
                {
                    Handler.MyAction=()=>Moving(recentDir, dir, directories);
                    Handler.Handle("finally Freed", "", "Q3Fault.txt", 10000, 3, reportPath);
                    /*
                     bool isNeedLock = true;
                     int i = 0;
                     bool isExceptionOccur = false;
                     while (isNeedLock)
                     {
                         i++;
                         try
                         {
                             Moving(recentDir,dir,directories)

                             isNeedLock = false;
                             if(isExceptionOccur)
                             {
                                 isExceptionOccur = false;
                                 var msg = new string[] { "File is available now " + DateTime.Now.ToString() };
                                 File.AppendAllLines(@"D:\Assignment\q3Fault.txt", msg);
                             }
                         }
                         catch (Exception ee)
                         {
                             isExceptionOccur = true;
                             Thread.Sleep(10000); //#sleep for 10 sec
                             isNeedLock = true;
                             if (i >= 20)
                             {
                                 isNeedLock = false;
                             }
                             var msg = new string[] { ee.Message + " " + DateTime.Now.ToString() };
                             File.AppendAllLines(@"D:\Assignment\q3Fault.txt", msg);
                         }
                     }
                    */
                }
            }
            void Moving(string recentDir,string dir,string[] directories)
            {
                if (recentDir != dir)
                {
                    Directory.Move(dir, toDir + @"\" + dir.Replace(fromDir, ""));

                }
                else if (directories.Length == 1)
                {
                    Directory.Move(dir, toDir + @"\" + dir.Replace(fromDir, ""));
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

         //  MoveFolders(ConfigurationManager.AppSettings["FromDir"], ConfigurationManager.AppSettings["ToDir"], ConfigurationManager.AppSettings["ReportPath"]);
           ProcessGenerator.waitTime = Convert.ToInt32(ConfigurationManager.AppSettings["setTimeInMinutes"]);
            ProcessGenerator.MyAction = () => MoveFolders(ConfigurationManager.AppSettings["FromDir"], ConfigurationManager.AppSettings["ToDir"], ConfigurationManager.AppSettings["ReportPath"]);
            Environment.ExitCode = WindowService.RunWindowsService("Q3 Data Moving", "Q1 Data Moving", "Q1 Data Moving with a timer period " + ProcessGenerator.waitTime + " min", "Q3");

        }
    }
}
