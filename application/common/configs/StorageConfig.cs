using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Application.Common.Configs
{
    public class StorageConfig
    {
        public long MaxFileSize { get; set; }
        public string AssetPath { get; set; }
        public string AssetPathRequest { get; set; }
    }
}