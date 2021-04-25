using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k173652_Q2_
{
    class Program
    {
        static void Main(string[] args)
        {
            var script = ConfigurationManager.AppSettings["directorypath"];


            ParseData obj = new ParseData();
            obj.FileParseAndRead(script);
            obj.StartParsing();
        }
    }
}
