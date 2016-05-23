using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Services.Media.Models;

namespace MyAndromeda.Services.Media
{
    public interface IMediaResizeService : IDependency
    {
        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="sizeList">The size list.</param>
        /// <returns></returns>
        ResizeSizeTaskContext ResizeImage(MemoryStream stream, LogoConfiguration configuration);

        IEnumerable<ResizeSizeTaskContext> ResizeImage(int andromedaSiteId, MemoryStream stream);

        IEnumerable<ResizeSizeTaskContext> ResizeLogoImage(MemoryStream stream, List<LogoConfiguration> sizeList);
    }
}