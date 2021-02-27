using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GemBox.Spreadsheet;
using MathModelingLab2.Models;

namespace MathModelingLab2.Services
{
    public class XlsService
    {
        private static readonly string RealDataPath = Directory.GetCurrentDirectory() + @"\XLSs\RealDataShrt.xlsx";

        public static List<RealDataTableViewRaw> ReadXls()
        {
            try
            {
                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                var workbook = ExcelFile.Load(RealDataPath);
                //TODO: convert to LINQ after debug

                return (from row in workbook.Worksheets.ElementAt(0).Rows
                    where row.AllocatedCells.ElementAt(0).ValueType != CellValueType.Null && row.AllocatedCells.ElementAt(0).ValueType != CellValueType.String
                    select new RealDataTableViewRaw
                    {
                        Year = Convert.ToDouble(row.AllocatedCells.ElementAt(0).Value),
                        X = Convert.ToDouble(row.AllocatedCells.ElementAt(1).Value),
                        Lx = Convert.ToDouble(row.AllocatedCells.ElementAt(2).Value),
                        Dx = Convert.ToDouble(row.AllocatedCells.ElementAt(3).Value),
                        Qx = Convert.ToDouble(row.AllocatedCells.ElementAt(4).Value),
                        CCx = Convert.ToDouble(row.AllocatedCells.ElementAt(7).Value)
                    }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}