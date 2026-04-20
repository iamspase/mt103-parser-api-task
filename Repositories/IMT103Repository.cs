using SwiftMT103ApiTask.Models;

namespace SwiftMT103ApiTask.Repositories
{
    public interface IMT103Repository
    {
        Task<long> SaveAsync(MT103Message message);
        Task<List<MT103Message>> GetAllAsync();
        Task<MT103Message> GetByIdAsync(long id);
    }
}
