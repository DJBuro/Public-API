using System;
using System.IO;
using System.Linq;

namespace MyAndromeda.Menus.Context.Thumbnails
{
    public class ResizeSizeTaskContext : IDisposable
    {
        public string Name { get; set; }

        public MemoryStream Result { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }
        
        public void Dispose()
        {
            this.Result.Close();
            this.Result.Dispose();
        }
    }
}