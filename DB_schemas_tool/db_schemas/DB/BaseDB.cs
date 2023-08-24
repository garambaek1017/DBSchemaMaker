using Dapper;
using DB_Schema_Maker.DB.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace DB_Schema_Maker.DB
{
    public abstract class BaseDB
    {
        #region 생성자 
        public BaseDB()
        {

        }
        public BaseDB(string _name)
        {
            DBName = _name;
            SaveExcelPath = Environment.CurrentDirectory + $"/{DBName}.xls";
        }
        #endregion

        // 엑셀 저장 패스 
        private string SaveExcelPath { get; set; }
        // 디비 이름 
        public string DBName { get; set; }

        /// <summary>
        /// 모든 테이블 정보 
        /// </summary>
        public List<string> AllTableList = new List<string>();

        /// <summary>
        /// 모든 엑셀 정보 
        /// </summary>
        public Dictionary<string, List<TableInfo>> AllTableInfos = new Dictionary<string, List<TableInfo>>();

        /// <summary>
        /// DB 별로 데이터 가져옴 
        /// </summary>
        public abstract void GetTableInfoFromDB();

        /// <summary>
        /// 테이블 리스트 가져옴 
        /// </summary>
        public void GetTableList(string tableListFileName)
        {
            var currentPath = Environment.CurrentDirectory;
            var filePath = currentPath + $"/list/{tableListFileName}.txt";
            var temp = System.IO.File.ReadAllLines(filePath);

            foreach (var t in temp)
            {
                AllTableList.Add(t);
            }

            Console.WriteLine($"{DBName} table count is {AllTableList.Count}");
        }

        /// <summary>
        /// Excel file 생성 
        /// </summary>
        public void WriteExcel()
        {
            Console.WriteLine($"Start Write Excel");

            Excel.Application excelApp = null;
            Excel.Workbook wb = null;

            excelApp = new Excel.Application();
            wb = excelApp.Workbooks.Add();

            var allTableInfosKeys = AllTableInfos.Keys.ToList();
            foreach (var key in allTableInfosKeys)
            {
                var newWorksheet = wb.Worksheets.Add(After: wb.Sheets[wb.Sheets.Count]) as Excel.Worksheet;
                newWorksheet.Name = key;

                // 첫번째 행 - 배경색, 두꺼운줄, font color, font bold 
                newWorksheet.get_Range("A1:G1").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Coral);
                newWorksheet.get_Range("A1:G1").Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThick;
                newWorksheet.get_Range("A1:G1").Font.Color = System.Drawing.Color.White;
                newWorksheet.get_Range("A1:G1").Font.Bold = true;
                newWorksheet.get_Range("A1:G1").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // 첫번째 설정 
                newWorksheet.Cells[1, 1] = "No";
                newWorksheet.Cells[1, 2] = "Name";
                newWorksheet.Cells[1, 3] = "Type";
                newWorksheet.Cells[1, 4] = "Length";
                newWorksheet.Cells[1, 5] = "Description";
                newWorksheet.Cells[1, 6] = "Index";
                newWorksheet.Cells[1, 7] = "Remark";

                // 열의 너비 설정 
                newWorksheet.Columns[1].ColumnWidth = 7;
                newWorksheet.Columns[2].ColumnWidth = 20;
                newWorksheet.Columns[3].ColumnWidth = 20;
                newWorksheet.Columns[4].ColumnWidth = 7;
                newWorksheet.Columns[5].ColumnWidth = 50;
                newWorksheet.Columns[6].ColumnWidth = 7;
                newWorksheet.Columns[7].ColumnWidth = 10;

                // 두번째줄부터 데이터 쓰기 
                int row = 2;
                var tableInfo = AllTableInfos[key];
                foreach (var tb in tableInfo)
                {
                    newWorksheet.Cells[row, 1] = tb.OridinalPosition;
                    newWorksheet.Cells[row, 2] = tb.ColumnName;
                    newWorksheet.Cells[row, 3] = tb.DataType;
                    newWorksheet.Cells[row, 4] = tb.Length;
                    newWorksheet.Cells[row, 5] = tb.Description;
                    newWorksheet.Cells[row, 6] = tb.Index;

                    row++;
                }

                // 마지막 줄 - 얇은 줄 설정 
                newWorksheet.get_Range($"A{row - 1}:G{row - 1}").Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            }

            //// 엑셀파일 저장
            wb.SaveAs(SaveExcelPath, Excel.XlFileFormat.xlWorkbookNormal);

            wb.Close(true);
            excelApp.Quit();

            Console.WriteLine("Making Excel is done");
        }

        /// <summary>
        /// DB로 부터 직접 쿼리하는 부분 
        /// </summary>
        /// <param name="conn"></param>
        protected void GetDataFromDB(IDbConnection conn)
        {
            // key : table 이름 
            foreach (var key in AllTableList)
            {
                var sql = "SELECT ORDINAL_POSITION as `OridinalPosition`, COLUMN_NAME as `ColumnName`, UPPER(DATA_TYPE) as `DataType`, REGEXP_SUBSTR(COLUMN_TYPE,'[0-9]+') as `Length`, COLUMN_COMMENT as `Description` , COLUMN_KEY as `Index`"
                          + " FROM "
                          + " INFORMATION_SCHEMA.COLUMNS "
                          + " WHERE "
                          + $" TABLE_SCHEMA = '{DBName}'"
                          + " AND "
                          + $"  TABLE_NAME = '{key}'; ";

                var result = conn.Query<TableInfo>(sql).OrderBy(w => w.OridinalPosition).ToList();

                AllTableInfos.Add(key, result);
            }
        }
        
    }
}
