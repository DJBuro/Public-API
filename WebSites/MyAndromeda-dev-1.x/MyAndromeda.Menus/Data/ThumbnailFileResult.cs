using System;
using System.Linq;

namespace MyAndromeda.Menus.Data
{
    public class ThumbnailFileResult
    {
        public ThumbnailFileResult(string fileName, string url) : this(fileName, url, string.Empty, string.Empty)
        {
            
        }
        
        public ThumbnailFileResult(string fileName, string url, string height, string width)
        {
            this.FileName = fileName;
            this.Url = url;
            this.Height = height;
            this.Width = width;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public string Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public string Width { get; set; }
    }
}