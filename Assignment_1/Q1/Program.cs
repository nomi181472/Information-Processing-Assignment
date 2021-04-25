using System;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;

namespace k173652_Q1
{
    class Program
    {
        static void Main(string[] args)
        {

            
                string regForUrl= @"^(http|http(s)?://)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?";
                 Regex reUrl = new Regex(regForUrl, RegexOptions.IgnoreCase);
                string regForDirectory = @"^(?:[a-zA-Z]\:(\\|\/)|file\:\/\/|\\\\|\.(\/|\\))([^\\\/\:\*\?\<\>\'\|]+(\\|\/){0,1})+$";
                Regex rePath = new Regex(regForDirectory,RegexOptions.IgnoreCase);
                string url = ConfigurationManager.AppSettings["url"];
                string script = ConfigurationManager.AppSettings["directorypath"];
            var currentTime=DateTime.Now.ToString().Substring(11).Replace(":","-").Replace(" ","-");
            if (reUrl.IsMatch((url)) && rePath.IsMatch((script)))
                {
                    WebClient wc = new WebClient();
                    DateTime today = DateTime.Today;
                    string FileName = currentTime+"Summary" + today.ToString("ddMMMyy") + ".html";
                    wc.DownloadFile(url, script + @"\" + FileName);
                    Console.WriteLine(script + @"\ - will contain a file named " + FileName);
                }
                else
                {
                    Console.WriteLine("URL or Path is uncorrect");
                    return;
                }
            

        }
    }
}
