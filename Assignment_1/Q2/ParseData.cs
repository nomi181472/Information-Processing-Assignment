using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace k173652_Q2_
{

    [XmlRoot("Root")]
    public class Scripts
    {
        [XmlElement("script")]
        public string script;
        [XmlElement("price")]
        public double price;

    }
   public class ParseData
    {
        private List<string> Categories { get; set; }
        private List<string> ColumnNames { get; set; }
        private string filePath { get; set; }
        private HtmlDocument document { get; set; }


        public ParseData()
        {
            Categories = new List<string>();
            ColumnNames = new List<string>();
            filePath = "";
            document = new HtmlDocument();

        }
        private List<string> GenerateCategories(HtmlNodeCollection cat)
        {



            List<string> name = new List<string>();


            foreach (var i in cat.Nodes())
            {
                name.Add(i.InnerText);

            }

            return name;
        }
        public void Serialize(List<Scripts> list, string path, string folder)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Scripts>));


            string xmlpath = String.Concat((path.Replace(@"/", "").Replace(@".", "_") + @"\" + folder.Replace(@"/", "").Replace(@".", "_") + ".xml").Where(c => !Char.IsWhiteSpace(c)));
            TextWriter writer = new StreamWriter(xmlpath);
            // Console.WriteLine(xmlpath);
            serializer.Serialize(writer, list);


        }
        private List<string> GenerateColumns(string col)
        {

            var columns = new List<string>();
            string str = "";

            for (int i = 0; i < col.Length; i++)
            {
                if (Char.IsWhiteSpace(col[i]))
                {
                    if (str != "")
                    {
                        columns.Add(str);

                        str = "";
                    }

                }
                else
                {
                    str += col[i];
                }

            }
            return columns;
        }
        private string CreateAndCheckDirective(string path)
        {
            string tpath = String.Concat((path).Where(c => !Char.IsWhiteSpace(c)));
            try
            {
                if (!Directory.Exists(tpath))
                {

                    DirectoryInfo di = Directory.CreateDirectory(tpath);


                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tpath;
        }
        public void StartParsing()
        {

            var ForFOlder = this.document.DocumentNode.SelectNodes("//div[@class='table-responsive']/table/tr/td");

            int index = this.ColumnNames.FindIndex((e => { if (e == "current") return true; else return false; }));
            int count = 0;

            bool scriptFlag = false;
            bool valuesFlag = false;
            bool parsedString = false;
            bool parsedValue = false;

            List<Scripts> AddressList = new List<Scripts>();
            string path = CreateAndCheckDirective(filePath.Substring(0, filePath.Length - 5));
            int counter = 0;
            double price = 0;
            string script = "";
            foreach (var i in ForFOlder.Nodes())
            {

                if (this.ColumnNames.Contains(i.InnerText.ToLower()) == false && (i.InnerText.All(c => Char.IsWhiteSpace(c)) == false))
                {
                    double d = -100;
                    bool flag = Double.TryParse(i.InnerText, out d);

                    if (flag == false)
                    {

                        scriptFlag = true;
                        script = i.InnerText.ToLower();
                        parsedString = true;

                    }
                    else
                    {

                        valuesFlag = true;
                        // Console.WriteLine(this.ColumnNames.Count);
                        if ((counter - this.ColumnNames.Count) % (index) == 0)
                        {

                            parsedValue = true;
                            price = d;
                            // Console.WriteLine(d);
                        }

                    }


                }
                else
                {
                    if (i.InnerText.All(c => Char.IsWhiteSpace(c)) == false && scriptFlag && valuesFlag)
                    {
                        string nestedpath = CreateAndCheckDirective(path + @"\" + Categories[count].Replace(@"/", "").Replace(@".", "_"));
                        this.Serialize(AddressList, nestedpath, Categories[count]);
                        AddressList = new List<Scripts>();
                        count++;
                        counter = counter - this.ColumnNames.Count;
                        scriptFlag = false;
                        valuesFlag = false;
                    }
                }
                if (parsedString && parsedValue)
                {
                    Scripts parsing = new Scripts();
                    parsing.price = price;
                    parsing.script = script;
                    parsedString = false;
                    parsedValue = false;
                    AddressList.Add(parsing);
                }
                counter++;
            }
            string nestedpat = this.CreateAndCheckDirective(path + @"\" + Categories[count]);

            this.Serialize(AddressList, nestedpat, Categories[count]);


        }
        public void FileParseAndRead(string filePath)
        {

            this.filePath = filePath;
            this.document.Load(this.filePath);
            var cat = document.DocumentNode.SelectNodes("//div[@class='table-responsive']/table/thead/tr/th/h4");
            this.Categories = this.GenerateCategories(cat);
            var node = document.DocumentNode.SelectSingleNode("//div[@class='table-responsive']/table/tr");
            ColumnNames = this.GenerateColumns(node.InnerText.ToString().ToLower());



        }


    }
}
