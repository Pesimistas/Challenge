using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Challenge
{
    class Program
    {
        static void Main(string[] args)
        {

            List<DataRow> dataList = new List<DataRow>();

            using (TextFieldParser csvParser = new TextFieldParser(@"C:\Users\User\Desktop\data.csv"))
            {

                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    var line = csvParser.ReadLine();
                    var values = line.Split(';');

                    dataList.Add(new DataRow(Int32.Parse(values[0]), values[1].Replace("\"",""), Int32.Parse(values[2]), values[3], Int32.Parse(values[4]), values[5], Int32.Parse(values[6]), Int32.Parse(values[7])));

                }

            }

            int max = findMax(dataList);

            int min = findMin(dataList);

            Console.WriteLine("Maximum value of childs_count column:" + max);
            thirdSubTask(dataList, max);

            Console.WriteLine("Minimum value of childs_count column:" + min);
            thirdSubTask(dataList, min);

            fourthSubTask(dataList);

            fifthSubTask(dataList);     

            Console.Read();

        }
        public static int findMax(List<DataRow> list)
        {
            return list.Max(s => s.childsCount);
        }

        public static int findMin(List<DataRow> list)
        {
            return list.Min(s => s.childsCount);
        }

        public static void thirdSubTask(List<DataRow> list, int value)
        {
            IEnumerable<DataRow> filter = (from a in list
                                           where a.childsCount == value
                                           select a);
            foreach (DataRow row in filter)
            {
                var split = row.typeLabel.Split(' ');
                Console.WriteLine(row.schoolName.Substring(0, 3) + "_" + split[1] + "-" + split[3] + "_" + row.lanLabel.Substring(0, 4));
            }
        }

        public static void fourthSubTask (List<DataRow> list)
        {
            List<KinderGarden> darz = new List<KinderGarden> { new KinderGarden(1,"Lietuviu",0), new KinderGarden(2,"Rusu",0), new KinderGarden(4,"Lenku",0), new KinderGarden(8,"Hebraju",0) };


            int allFreeSpaces = (from a in list
                                   select a.freeSpace).Sum();

            for(int i = 0; i < darz.Count(); ++i)
            {
                darz[i].totalFreeSpace = ( from a in list
                                         where a.lanId == darz[i].lanId
                                         select a.freeSpace).Sum();
            }

            darz = darz.OrderByDescending(o => o.totalFreeSpace).ToList();


            double percents = (double) darz[0].TotalFreeSpace / allFreeSpaces;


            Console.WriteLine(darz[0].typeLabel + " kalbos darzeliai turi daugiausia laisvu vietu procentais:"  + percents.ToString("p2"));

        }

        public static void fifthSubTask(List<DataRow> list) {

            Console.WriteLine("Darzeliai atitinkantys salyga:");

            IEnumerable<DataRow> filter = (from a in list
                                    where a.freeSpace >= 2 && a.freeSpace <= 4
                                    select a);

            var grouped  = (from s in filter
                            orderby s.schoolName descending
                            group s by s.schoolName);


            foreach (var groupName in grouped)
            {
                Console.WriteLine(groupName .Key);
            }

        }

        public class DataRow
        {
            public int darzId;
            public string schoolName;
            public int typeId;
            public string typeLabel;
            public int lanId;
            public string lanLabel;
            public int childsCount;
            public int freeSpace;

            public DataRow(int p1, string p2, int p3, string p4, int p5, string p6, int p7, int p8)
            {
                darzId = p1;
                schoolName = p2;
                typeId = p3;
                typeLabel = p4;
                lanId = p5;
                lanLabel = p6;
                childsCount = p7;
                freeSpace = p8;

            }

        }
        public class KinderGarden
        {
            public int lanId { get; set; }
            public string typeLabel { get; set; }
            public int totalFreeSpace { get; set; }

            public int TotalFreeSpace
            {
                get { return totalFreeSpace; }
                set { totalFreeSpace = value; }
            }


            public KinderGarden(int k1,string k2,int k3)
            {
                lanId = k1;
                typeLabel = k2;
                totalFreeSpace = k3;
            }
        }
    }
}
