using k173652_Q1;
using k173652_Q1_;
using k173652_Q2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace k173652_Q4
{
    public class ScriptName
    {
        public ScriptData scriptData { get; set; }
    }
    public class ScriptData
    {
        public string lastUpdatedOn { get; set; }
        public List<Data> data { get; set; }

    }
    public class Data
    {
        public string date { get; set; }
        public double price { get; set; }

    }
    public class Scripts
    {


        public string script { get; set; }

        public double price { get; set; }

    }
    class Program
    {
        static public void XMLToJSON(string xmlDirectory, string jsonDirectory,string reportPath)
        {


            foreach (var dir in GetSubDirectories(xmlDirectory))
            {

                foreach (var xmlSub in GetSubDirectories(dir))
                {

                    string jsonSub = xmlSub.Replace(dir, jsonDirectory);
                    string[] files;
                    try
                    {
                        files = Directory.GetFiles(jsonSub, "*.json").ToArray();
                    }
                    catch (Exception ee)
                    {
                        files = Directory.GetFiles(jsonDirectory, "*.json").ToArray();
                    }
                    var xmlFiles = Directory.GetFiles(xmlSub, "*.xml")[0];
                    List<Scripts> listofscript = DeserializeXML(xmlFiles);
                    foreach (var script in listofscript)
                    {

                        string p = PathBuilder(script, jsonSub);
                        Handler.MyAction = () => SerilizeAndDeserilize(files, p, jsonSub, script);
                        Handler.Handle("finally Freed", "", "Q4Fault.txt", 10000, 3, reportPath);
                       

                        /*                       
                        bool isNeedLock = true;
                        int i = 0;
                        while (isNeedLock)
                        {
                            i++;
                            try
                            {

                                if (!files.Contains(p))
                                {
                                    Directory.CreateDirectory(jsonSub);
                                    ScriptName sd = new ScriptName();
                                    sd.scriptData = new ScriptData()
                                    {
                                        lastUpdatedOn = DateTime.Now.ToString(),
                                        data = new List<Data>() {
                                   new Data()
                                {
                                     price = script.price,
                                date = DateTime.Now.ToString()
                                }
                                }
                                    };



                                    SerilizeIt(sd, p);
                                }
                                else
                                {


                                    ScriptName sn = DeserilizeIt(p);

                                    sn.scriptData.data.Add(
                                        new Data()
                                        {
                                            price = script.price,
                                            date = DateTime.Now.ToString()
                                        });
                                    sn.scriptData.lastUpdatedOn = DateTime.Now.ToString();
                                    SerilizeIt(sn, p);


                                }
                                isNeedLock = false;
                               // var msg = new string[] { "Finally Freed " + DateTime.Now.ToString() };
                                //File.AppendAllLines(@"D:\Assignment\q4Success.txt", msg);

                            }
                            catch (IOException ioexceptip)
                            {
                                Thread.Sleep((10000)); //#sleep for 10 sec
                                isNeedLock = true;
                                if (i >= 20)
                                {
                                    isNeedLock = false;
                                }
                                var msg = new string[] { ioexceptip.Message + " " + DateTime.Now.ToString() };
                                File.AppendAllLines(@"D:\Assignment\q4Fault.txt", msg);
                            }
                        }



                    */
                        }
                        
                    File.Delete(xmlFiles);
                    Directory.Delete(xmlSub);

                        
                }
                Directory.Delete(dir);
            }
            void SerilizeAndDeserilize(string[] files,string p,string jsonSub,Scripts script)
            {
                if (!files.Contains(p))
                {
                    Directory.CreateDirectory(jsonSub);
                    ScriptName sd = new ScriptName();
                    sd.scriptData = new ScriptData()
                    {
                        lastUpdatedOn = DateTime.Now.ToString(),
                        data = new List<Data>() {
                                   new Data()
                                {
                                     price = script.price,
                                date = DateTime.Now.ToString()
                                }
                                }
                    };



                    SerilizeIt(sd, p);
                }
                else
                {


                    ScriptName sn = DeserilizeIt(p);

                    sn.scriptData.data.Add(
                        new Data()
                        {
                            price = script.price,
                            date = DateTime.Now.ToString()
                        });
                    sn.scriptData.lastUpdatedOn = DateTime.Now.ToString();
                    SerilizeIt(sn, p);


                }
            }
            
            string PathBuilder(Scripts scr, string jsSub)
            {
                return jsSub + @"\" + scr.script.Replace(' ', '-').Replace('.', '-') + ".json";
            }
            void SerilizeIt(ScriptName obj, string path)
            {
                using (var file = File.CreateText(path))
                {
                    var json = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
                    file.Write(json);
                }
            }
            ScriptName DeserilizeIt(string path)
            {
                return JsonConvert.DeserializeObject<ScriptName>(File.ReadAllText(path));
            }
            List<Scripts> DeserializeXML(string path)
            {
                List<Scripts> list = new List<Scripts>();
                XmlSerializer serializer = new XmlSerializer(typeof(List<Scripts>));
                StreamReader reader = new StreamReader(path);
                list = (List<Scripts>)serializer.Deserialize(reader);

                reader.Close();
                return list;
            }

            string[] GetSubDirectories(string currentDir)
            {
                return Directory.GetDirectories(currentDir);
            }
        }

        static void Main(string[] args)
        {

           // XMLToJSON(ConfigurationManager.AppSettings["xmlDirectory"], ConfigurationManager.AppSettings["jsonDirectory"],ConfigurationManager.AppSettings["ReportPath"]);
           ProcessGenerator.waitTime = Convert.ToInt32(ConfigurationManager.AppSettings["setTimeInMinutes"]);
            ProcessGenerator.MyAction = () => XMLToJSON(ConfigurationManager.AppSettings["xmlDirectory"], ConfigurationManager.AppSettings["jsonDirectory"], ConfigurationManager.AppSettings["ReportPath"]);//
           Environment.ExitCode = WindowService.RunWindowsService("Q4 Data XML TO Json", "Q4 Data XML TO Json", "Q4 Data XML TO Json " + ProcessGenerator.waitTime + " min", "Q4");

            
        }
    }
}
