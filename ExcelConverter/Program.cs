using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExcelConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var workbookPath = Path.Combine(dir, "PLIB APIs.xlsx");
            var excelReader = new ExcelReader(workbookPath);
            var excelTable = excelReader.Read();
            var collection = (new ExcelAdapter()).Convert(excelTable);
            Console.WriteLine("Converting Excel to text file...");
            var writer = new TextWriter(Path.Combine(dir, "PLIB APIs.txt"));
            writer.Write(collection);
            Console.WriteLine("Conversion completed.");
        }
    }
}
