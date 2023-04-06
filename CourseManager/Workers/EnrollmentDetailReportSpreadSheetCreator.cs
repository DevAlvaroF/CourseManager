using CourseManager.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace CourseManager.Workers
{
    internal class EnrollmentDetailReportSpreadSheetCreator
    {
        public void Create(string fileName, IList<EnrollmentDetailReportModel> enrollmentModels)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                // Get JSON from Models List
                string json = JsonConvert.SerializeObject(enrollmentModels);

                // Deserialize JSON into a DataTable
                DataTable enrollmentsTable = (DataTable)JsonConvert.DeserializeObject<DataTable>(json);
                //DataTable enrollmentsTable = (DataTable)JsonConvert.DeserializeObject(json, typeof(DataTable));

                // Create Excel Workbook
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Create workseeht
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

                // Add Data to worksheet
                SheetData sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                // Create list of sheets to add 
                Sheets sheetList = document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                // Create and Link sheet to 
                Sheet singleSheet = new Sheet()
                {
                    Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Report Sheet"
                };
                sheetList.Append(singleSheet);

                // Create Table Headers
                Row excelTitleRow = new Row();

                foreach (DataColumn tableCol in enrollmentsTable.Columns)
                {
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(tableCol.ColumnName);
                    excelTitleRow.Append(cell);
                }
                sheetData.AppendChild(excelTitleRow);

                // Create Table Headers
                foreach (DataRow tableRow in enrollmentsTable.Rows)
                {
                    Row excelNewRow = new Row();
                    foreach (DataColumn tableCol in enrollmentsTable.Columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(tableRow[tableCol.ColumnName].ToString());
                        excelNewRow.Append(cell);
                    }
                    sheetData.AppendChild(excelNewRow);
                }

                workbookPart.Workbook.Save();

            }

        }

    }
}
