using Dapper;
using DBSchemaMaker.DB.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace DBSchemaMaker.DB
{
    public abstract class BaseDB
    {
        #region 생성자 
        public BaseDB()
        {
        }
        public BaseDB(string name)
        {
            _dbName = name;
            _saveExcelPath = Environment.CurrentDirectory + $"/{_dbName}.xlsx";
        }
        #endregion

        private string _saveExcelPath { get; set; }
        private List<string> _allTableList = new List<string>();
        private Dictionary<string, List<TableInfo>> _allTableInfos = new Dictionary<string, List<TableInfo>>();
        private string _dbName { get; set; }
        public abstract void GetTableInfoFromDB();
        public abstract void TestConnection();

        public void DoTestQuery(IDbConnection conn)
        {
            conn.Query("SELECT 1");
            LogHelper.Debug($"{_dbName}:: DoTestQuery is Success");
        }

        public void WriteExcel()
        {
            LogHelper.Debug($"{_dbName}:: Start Write Excel");

            Excel.Application excelApp = null;
            Excel.Workbook wb = null;

            excelApp = new Excel.Application();
            wb = excelApp.Workbooks.Add();

            var allTableInfosKeys = _allTableInfos.Keys.ToList();
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
                var tableInfo = _allTableInfos[key];
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

            if (File.Exists(_saveExcelPath))
            {
                // 파일이 존재하면 삭제
                File.Delete(_saveExcelPath);
            }

            // 엑셀파일 저장
            wb.SaveAs(_saveExcelPath, Excel.XlFileFormat.xlWorkbookNormal, AccessMode: Excel.XlSaveAsAccessMode.xlNoChange);
            wb.Close(true);

            excelApp.Quit();

            LogHelper.Debug($"{_dbName}:: Making Excel is done --> {_saveExcelPath}");
        }
        protected void GetTableInfoFromDB(IDbConnection conn)
        {
            // key : table 이름 
            foreach (var key in _allTableList)
            {
                var sql = "SELECT ORDINAL_POSITION as `OridinalPosition`, COLUMN_NAME as `ColumnName`, UPPER(DATA_TYPE) as `DataType`, REGEXP_SUBSTR(COLUMN_TYPE,'[0-9]+') as `Length`, COLUMN_COMMENT as `Description` , COLUMN_KEY as `Index`"
                          + " FROM "
                          + " INFORMATION_SCHEMA.COLUMNS "
                          + " WHERE "
                          + $" TABLE_SCHEMA = '{_dbName}'"
                          + " AND "
                          + $"  TABLE_NAME = '{key}'; ";

                var result = conn.Query<TableInfo>(sql).OrderBy(w => w.OridinalPosition).ToList();

                _allTableInfos.Add(key, result);
            }

            LogHelper.Debug($"{_dbName}:: GetTableInfoFromDB is Success");
        }
        protected void GetAllTableListFromDB(IDbConnection conn)
        {
            var sql = $"SHOW TABLES FROM {_dbName}";
            _allTableList = conn.Query<string>(sql).ToList();

            LogHelper.Debug($"{_dbName}:: GetAllTableListFromDB is Success");
        }
    }
}
