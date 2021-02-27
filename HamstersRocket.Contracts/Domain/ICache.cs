using System.Threading.Tasks;

namespace HamstersRocket.Contracts.Domain
{
    public interface ICache<T>
    {
        Task<T[]> GetAllAsync();
        Task SaveAsync(T value);
        Task ClearAsync();
    }
}
