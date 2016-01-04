using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MyAndromeda.Data.DataWarehouse.Domain.Marketing;
using MyAndromeda.ImportExport.Helpers;

namespace MyAndromeda.ImportExport.Customers
{
    /// <summary>
    /// Response type: "text/comma-separated-values", "filename.csv"
    /// </summary>
    public class ExportCustomerList : IExportCustomerList
    {
        private readonly FieldValue<CustomerModel> titleField = new FieldValue<CustomerModel>("Title", e => e.Title);
        private readonly FieldValue<CustomerModel> firstName = new FieldValue<CustomerModel>("FirstName", e => e.FirstName);
        private readonly FieldValue<CustomerModel> surName = new FieldValue<CustomerModel>("SurName", e => e.Surname);
        private readonly FieldValue<CustomerModel> email = new FieldValue<CustomerModel>("Email", e => e.Email);

        private FieldValue<CustomerModel>[] columns; 

        public ExportCustomerList()
        {
        }

        public MemoryStream GetCsv(IEnumerable<CustomerModel> customers)
        {
            MemoryStream output = new MemoryStream();
            StreamWriter writer = new StreamWriter(output, Encoding.UTF8);

            this.CreateHeader(writer);
            this.CreateBody(writer, customers);

            writer.Flush();
            output.Position = 0;

            return output;
        }

        internal FieldValue<CustomerModel>[] Columns()
        {
            return this.columns ?? (this.columns = new[]
            {
                this.titleField,
                this.firstName,
                this.surName,
                this.email
            });
        }

        private void CreateHeader(StreamWriter writer) 
        {
            //writer.WriteCsvHeaderValue(titleField.FieldName);
            //writer.WriteCsvHeaderValue(firstName.FieldName);
            //writer.WriteCsvHeaderValue(surName.FieldName);
            //writer.WriteCsvHeaderValue(email.FieldName, true);
            var columns = this.Columns();
            
            for (var i = 0; i < columns.Length; i++)
            {
                var isLast = i == columns.Length - 1;
                var column = columns[i];
                writer.WriteCsvHeaderValue(column.FieldName, isLast);
            }

            writer.WriteLine();
        }

        private void CreateBody(StreamWriter writer, IEnumerable<CustomerModel> customers)
        {
            var columns = this.Columns();

            foreach (var customer in customers)
            {
                //writer.WriteCsvLineValue(titleField.GetValue(customer));
                //writer.WriteCsvLineValue(firstName.GetValue(customer));
                //writer.WriteCsvLineValue(surName.GetValue(customer));
                //writer.WriteCsvLineValue(email.GetValue(customer), true);
                for (var i = 0; i < columns.Length; i++)
                {
                    var isLast = i == columns.Length - 1;
                    var column = columns[i];
                    writer.WriteCsvLineValue(column.GetValue(customer), isLast);
                }

                writer.WriteLine();
            }
        }
    }
}