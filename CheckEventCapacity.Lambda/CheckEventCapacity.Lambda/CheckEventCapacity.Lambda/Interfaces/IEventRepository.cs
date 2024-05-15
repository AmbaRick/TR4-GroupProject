using CheckEventCapacity.Lambda.Entities;

namespace CheckEventCapacity.Lambda.Interfaces
{

    /// <summary>
    /// interface to set up methods available for Event Repository
    /// </summary>
    public interface IEventRepository
    {
        Task<Event?> Get(string id);

    }
}
