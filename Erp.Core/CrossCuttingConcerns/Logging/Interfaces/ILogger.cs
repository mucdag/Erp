using Core.CrossCuttingConcerns.Logging.Models;
using Core.Utilities.WebApi.Infrastructure;

namespace Core.CrossCuttingConcerns.Logging.Interfaces
{
    public interface ILogger
    {
        void Add<T>(T obj) where T : LogBase;
        ApiQueryResult<T> Get<T>(LogQueryOption logQueryOption) where T : LogBase, new();
    }
}