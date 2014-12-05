using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domotica
{
    public class clsOPCCSV
    {
        public string Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public static void SaveCSV(String path, List<clsOPCCSV> list)
        {
            var writer = new StreamWriter(path);
            foreach (clsOPCCSV item in list)
            {
                writer.WriteLine(String.Format("{0},{1},{2}", item.Id, item.X, item.Y));
            }
            writer.Close();
        }
        public static List<clsOPCCSV> ReadCSV(String path)
        {
            List<clsOPCCSV> list = new List<clsOPCCSV>();
            var reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                list.Add(new clsOPCCSV() { Id = values[0], X = Double.Parse(values[1]), Y = Double.Parse(values[2]) });
            }
            reader.Close();
            return list;
        }
        public static clsOPCCSV ReadCSV(String path, String id)
        {
            if (File.Exists(path))
            {
                foreach (clsOPCCSV item in ReadCSV(path))
                {
                    if (item.Id.Equals(id))
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}
