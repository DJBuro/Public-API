using System.Drawing;

namespace MyAndromeda.Services.Media.Models
{
    public class LogoConfiguration
    {
        public LogoConfiguration(int width, int height, ContentAlignment alignment)
        {
            this.Width = width;
            this.Height = height;
            this.Alignment = alignment;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public ContentAlignment Alignment { get; set; }
    }
}