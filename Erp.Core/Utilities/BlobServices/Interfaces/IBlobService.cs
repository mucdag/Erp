using Core.Utilities.BlobServices.Concrete;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Core.Utilities.BlobServices.Interfaces
{
    public interface IBlobService
    {
        Task<string> SaveAsync(string base64, string fileName);
        FileStream Get(string fileName);
        void Delete(string fileName);
        Stream CreateExcelFile<T>(List<ExcelFileSheetConfig<T>> sheetConfigs) where T : class, new();
    }
}