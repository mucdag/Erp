using System.Collections.Generic;

namespace Core.Utilities.WebApi.Infrastructure
{
    public class ApiQueryResult<T>
    {
        public ApiQueryResult()
        {
        }

        public ApiQueryResult(List<T> items, int? count)
        {
            Items = items;
            Count = count;
        }

        public List<T> Items { get; set; }
        public int? Count { get; set; }
    }
}
