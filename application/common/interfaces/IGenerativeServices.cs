using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Models;

namespace MyApi.application.common.interfaces
{
    public interface IGenerativeServices
    {
        Task<string> RetrieveImagesInfo(List<Image> images);

        Task<object> GetGenerativeConfig();
    }
}
