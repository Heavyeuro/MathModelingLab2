using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GemBox.Spreadsheet;
using MathModelingLab2.Models;

namespace MathModelingLab2.Services
{
    public class XLSService
    {
        private static string realDataPath = Directory.GetCurrentDirectory() + @"XLSs\RealData.xls";

        public RealDataTableView ReadXLS()
        {
            var realDataTableView = new RealDataTableView();

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = ExcelFile.Load(realDataPath);

            realDataTableView.RealDataTableClustersEnumerable = new List<RealDataTableView.RealDataTableClusters>
            {
                new()
                {
                    Sex = "Woman",
                    RealDataTableViewRaws = ExcelWorksheet(workbook.Worksheets.ElementAt(0))
                },
                new()
                {
                    Sex = "Man",
                    RealDataTableViewRaws = ExcelWorksheet(workbook.Worksheets.ElementAt(1))
                }
            };

            return realDataTableView;
        }

        private static IEnumerable<RealDataTableView.RealDataTableViewRaw> ExcelWorksheet(ExcelWorksheet excelWorksheet)
        {
            var data = new List<RealDataTableView.RealDataTableViewRaw>();
            foreach (var row in excelWorksheet.Rows)
            {
                if (row.AllocatedCells.ElementAt(2).ValueType == CellValueType.Null ||
                    row.AllocatedCells.ElementAt(2).ValueType == CellValueType.String) continue;

                data.Add(new RealDataTableView.RealDataTableViewRaw
                {
                    Year = Convert.ToDouble(row.AllocatedCells.ElementAt(1).Value),
                    X = Convert.ToDouble(row.AllocatedCells.ElementAt(2).Value),
                    Lx = Convert.ToDouble(row.AllocatedCells.ElementAt(3).Value),
                    Dx = Convert.ToDouble(row.AllocatedCells.ElementAt(4).Value),
                    Qx = Convert.ToDouble(row.AllocatedCells.ElementAt(5).Value),
                    CCx = Convert.ToDouble(row.AllocatedCells.ElementAt(8).Value)
                });
            }

            return data;
        }
    }
}