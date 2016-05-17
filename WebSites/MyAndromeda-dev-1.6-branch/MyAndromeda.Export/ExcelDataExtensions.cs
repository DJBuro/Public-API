using Newtonsoft.Json;
using System;
namespace MyAndromeda.Export
{
    public static class ExcelDataExtensions
    {
        public static void WriteOutHeader( 
            DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadsheet,
            DocumentFormat.OpenXml.Spreadsheet.Worksheet worksheet,
            dynamic modelObject)
        {
            
            for (int mdx = 0; mdx < modelObject.Count; mdx++)
            {
                if (modelObject[mdx].title == null && modelObject[mdx].field == null)
                    continue;
                // If the column has a title, use it.  Otherwise, use the field name.
                ExcelBuilderExtensions.SetColumnHeadingValue(spreadsheet, worksheet, Convert.ToUInt32(mdx + 1),
                    modelObject[mdx].title == null ? modelObject[mdx].field.ToString() : modelObject[mdx].title.ToString(),
                    false, false);

                // Is there are column width defined?
                ExcelBuilderExtensions.SetColumnWidth(
                    worksheet, 
                    mdx + 1, modelObject[mdx].width != null
                    ? Convert.ToInt32(modelObject[mdx].width.ToString()) / 4
                    : 25);
            }
        }

        public static void WriteOutData(
            DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadsheet,
            DocumentFormat.OpenXml.Spreadsheet.Worksheet worksheet,
            dynamic modelObject,
            dynamic dataObject)
        {
            // For each row of data...
            for (int idx = 0; idx < dataObject.Count; idx++)
            {
                // For each column...
                for (int mdx = 0; mdx < modelObject.Count; mdx++)
                {
                    if (modelObject[mdx].title == null && modelObject[mdx].field == null)
                        continue;
                    // Set the field value in the spreadsheet for the current row and column.
                    ExcelBuilderExtensions.SetCellValue(spreadsheet, worksheet, Convert.ToUInt32(mdx + 1), Convert.ToUInt32(idx + 2),
                        dataObject[idx][modelObject[mdx].field.ToString()].ToString(),
                        false, false);
                }
            }
        }
    }
}