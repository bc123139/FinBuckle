using System.Threading.Tasks;

namespace FinBuckleMvc.Services
{
    public interface IHomeService
    {
        Task<string> GetResult();
    }
}
