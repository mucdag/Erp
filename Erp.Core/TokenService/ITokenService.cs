using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Core.TokenService
{
    public interface ITokenService<T> where T : class
    {
        string BuildToken(T userData);
    }
}
