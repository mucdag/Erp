using System.Collections.Generic;

namespace Core.Utilities.BlobServices.Concrete
{
    public class ExcelFileSheetConfig<T>
    {
        public string SheetName { get; set; }
        public List<T> Values { get; set; }
        public List<ExcelFileSheetColumnConfig> Columns { get; set; }
    }
    public class ExcelFileSheetColumnConfig
    {
        public string ColumnAddress { get; set; }
        public string ColumnName { get; set; }
    }
    public static class FileContentType
    {
        public const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
}
