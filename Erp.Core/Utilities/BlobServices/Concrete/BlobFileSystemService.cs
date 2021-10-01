using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Core.Settings;
using Core.Utilities.BlobServices.Interfaces;
using Microsoft.Extensions.Options;
using OfficeOpenXml;

namespace Core.Utilities.BlobServices.Concrete
{
    public class BlobFileSystemService : IBlobService
    {
        private readonly string _basePath;

        public BlobFileSystemService(IOptions<AppSettings> appSettings)
        {
            _basePath = appSettings.Value.FolderPath;
        }

        public async Task<string> SaveAsync(string base64, string fileName)
        {
            var name = GetFileName(fileName);
            var extension = GetFileExtension(fileName);

            var path = Path.Combine(_basePath, extension.ToLower());
            var fileUniqueName = $"{name}-{Guid.NewGuid()}.{extension}";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var constWord = "base64,";
            var result = base64.LastIndexOf(constWord, StringComparison.Ordinal);

            if (result > -1)
                result = result + constWord.Length;

            var bytes = Convert.FromBase64String(base64.Substring(result));

            using (var fileStream = new FileStream(Path.Combine(path, fileUniqueName), FileMode.Create))
            {
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
                await fileStream.FlushAsync();
            }

            return fileUniqueName;
        }

        public FileStream Get(string fileName)
        {
            var extension = GetFileExtension(fileName);
            var path = Path.Combine(_basePath, extension.ToLower(), fileName);

            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        public void Delete(string fileName)
        {
            var extension = GetFileExtension(fileName);
            var path = Path.Combine(_basePath, extension.ToLower(), fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private string GetFileName(string fileFullName)
        {
            var lastIndexOf = fileFullName.LastIndexOf('.');

            return fileFullName.Substring(0, lastIndexOf);
        }

        private string GetFileExtension(string fileFullName)
        {
            var lastIndexOf = fileFullName.LastIndexOf('.');
            return fileFullName.Substring(lastIndexOf + 1);
        }

        /// <summary>
        /// sheetConfigs e eklenen sheet kadar tablar oluşturulur.
        /// </summary>
        /// <param name="sheetConfigs">SheetName => sheet adı,Values => sheet içerisindeki değerleri oluşturulur gönderilen class'ın property sırasına göre column lar sıra ile oluşturulur
        /// ,Columns => ilk satırdaki column alanı </param>
        /// <returns></returns>
        public Stream CreateExcelFile<T>(List<ExcelFileSheetConfig<T>> sheetConfigs) where T : class, new()
        {
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                foreach (var sheet in sheetConfigs)
                {
                    var workSheet = package.Workbook.Worksheets.Add(sheet.SheetName);
                    workSheet.Cells.LoadFromCollection(sheet.Values, true);
                    foreach (var column in sheet.Columns)
                    {
                        workSheet.Cells[column.ColumnAddress].Value = column.ColumnName;
                    }
                }
                package.Save();
            }
            stream.Position = 0;
            return stream;
        }
    }
}