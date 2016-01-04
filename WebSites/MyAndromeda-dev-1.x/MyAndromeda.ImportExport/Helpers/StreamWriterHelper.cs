using System.IO;

namespace MyAndromeda.ImportExport.Helpers
{
    public static class StreamWriterHelper 
    {
        public static void WriteCsvHeaderValue(this StreamWriter writer, string title, bool last = false) 
        {
            writer.Write(title);
              
            if (last)
                return;

            writer.Write(",");
        }

        public static void WriteCsvLineValue(this StreamWriter writer, object value, bool last = false) 
        {
            writer.Write("\"");
            writer.Write(value);
            writer.Write("\"");  
            if (last)
                return;

            writer.Write(",");
        }
    }
}