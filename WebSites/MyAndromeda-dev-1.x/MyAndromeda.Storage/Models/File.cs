using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Storage.Models
{
    public enum FileType
    {
        File,
        Directory
    }

    public class FileModel
    {
        public FileModel(string name, FileType fileType, long size)
        {
            this.Name = name;
            this.FileType = fileType == Models.FileType.Directory ? "d" : "f";
            this.Size = size;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string FileType { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        
    }
}
