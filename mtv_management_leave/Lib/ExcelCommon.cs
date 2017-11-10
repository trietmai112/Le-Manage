using Microsoft.Office.Interop.Excel;
using mtv_management_leave.Lib.Extendsions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib
{
    //[SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
    public class ExcelCommon
    {
        Application excelApp = new Application();
        _Worksheet workSheet = null;
        List<string> columnMap = new List<string>();
        _Workbook workBook = null;
        int currentRow = 1;
        public ExcelCommon()
        {
            //excelApp.Visible = true;
            workBook = excelApp.Workbooks.Add();
            workSheet = (Worksheet)excelApp.ActiveSheet;
        }

        public void CreateNewShet(string sheetName)
        {
            if (sheetName == null) sheetName = DateTime.Now.ToString("MM/dd/yyyy");
            workSheet.Name = sheetName;
        }
        public void AddHeader<T>()
        {
            var type = typeof(T);
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public 
                | System.Reflection.BindingFlags.Instance 
                | System.Reflection.BindingFlags.GetProperty 
                | System.Reflection.BindingFlags.SetField);
            int column = 1;
            foreach(var prop in properties)
            {
                var columnName = prop.Name;
                if (Attribute.IsDefined(prop, typeof(DisplayAttribute)))
                {
                    var dis = Attribute.GetCustomAttribute(prop, typeof(DisplayAttribute)) as DisplayAttribute;
                    columnName = dis.Name;
                }
                workSheet.Cells[currentRow, column] = columnName;
                workSheet.Columns[column].AutoFit();
                columnMap.Add(prop.Name);
                column++;
            }
        }

        public void AddRecords<T>(IEnumerable<T> records)
        {
            foreach (var item in records) AddRecord<T>(item);
        }

        public void AddRecord<T>(T record)
        {
            if (record == null) return;
            int column = 1;
            currentRow += 1;
            var type = record.GetType();
            foreach(var item in columnMap)
            {
                var prop = type.GetProperty(item);
                workSheet.Cells[currentRow, (char)column] = prop.GetValue(record).vToString();
                column++;
            }
            workSheet.Columns.AutoFit();
        }

        public void Save(string path)
        {
            workSheet.SaveAs(path);
            workBook.Close();
            excelApp.Application.Quit();
            excelApp.Quit();
            //excelApp.ThisWorkbook.Close();
        }

        public Stream Save()
        {
            MemoryStream memoryStream = new MemoryStream();
            workBook.SaveAs(memoryStream);
            workBook.Close();
            excelApp.Application.Quit();
            excelApp.Quit();
            return memoryStream;
        }
    }
}
