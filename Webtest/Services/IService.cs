using System.Collections.Generic;
using System.Threading.Tasks;

namespace Webtest
{
    public interface IService
    {
        Task<string> CreateLetterAsync(string request);
    }
}