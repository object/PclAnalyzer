using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace ExcelConverter
{
    class ExcelReader
    {
        public class ExcelTable
        {
            public IList<string> Headers { get; set; }
            public IList<IList<string>> Data { get; set; }

            public ExcelTable()
            {
                this.Headers = new List<string>();
                this.Data = new List<IList<string>>();
            }
        }

        private readonly string _path;

        public ExcelReader(string path)
        {
            _path = path;
        }

        public ExcelTable Read()
        {
            var application = new Application();
            var workbook = application.Workbooks.Open(_path);
            var worksheet = workbook.Worksheets[1] as Worksheet;
            var range = worksheet.UsedRange;
            var valueArray = (object[,])range.get_Value(XlRangeValueDataType.xlRangeValueDefault);
            var table = new ExcelTable();

            for (int header = 1; header <= valueArray.GetLength(1); header++)
            {
                table.Headers.Add(valueArray[1, header].ToString());
            }

            for (int row = 2; row <= valueArray.GetLength(0); row++)
            {
                var dataRow = new List<string>();
                for (int col = 1; col <= valueArray.GetLength(1); col++)
                {
                    var value = valueArray[row, col];
                    dataRow.Add(value == null ? null : value.ToString());
                }
                table.Data.Add(dataRow);
            }

            workbook.Close();
            Marshal.ReleaseComObject(workbook);
            return table;
        }
    }
}