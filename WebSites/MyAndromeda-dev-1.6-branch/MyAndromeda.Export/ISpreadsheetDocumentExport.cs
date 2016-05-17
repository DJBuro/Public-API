using System;
using System.IO;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Logging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyAndromeda.Export
{
    public interface ISpreadsheetDocumentExport : ITransientDependency
    {
        byte[] Export(string model, string data, string title);
    }

    public class SpreadsheetExport : ISpreadsheetDocumentExport
    {
        private readonly IMyAndromedaLogger logger;

        public SpreadsheetExport(IMyAndromedaLogger logger) 
        {
            this.logger = logger;
        }

        public byte[] Export(string model, string data, string title)
        {
            byte[] file = null;

            this.logger.Info("building excel document");

            try
            {
                using (var stream = new MemoryStream())
                {
                    SpreadsheetDocument spreadsheet = stream.CreateWorkbook();

                    spreadsheet.AddBasicStyles();
                    spreadsheet.AddAdditionalStyles();
                    spreadsheet.AddWorksheet(title);

                    Worksheet worksheet = spreadsheet.WorkbookPart.WorksheetParts.First().Worksheet;

                    var modelObject = JsonConvert.DeserializeObject<dynamic>(model);
                    var dataObject = JsonConvert.DeserializeObject<dynamic>(data);


                    ExcelDataExtensions.WriteOutHeader(spreadsheet, worksheet, modelObject);
                    ExcelDataExtensions.WriteOutData(spreadsheet, worksheet, modelObject, dataObject);

                    worksheet.Save();
                    spreadsheet.Close();

                    file = stream.ToArray();
                }
            }
            catch (Exception e)
            {
                this.logger.Error("Failed producing the excel document");
                this.logger.Error("model: {0}", model);
                this.logger.Error("data: {0}", data);
                this.logger.Error(e);

                throw;
            }

            

            return file;
        }
    }
}
