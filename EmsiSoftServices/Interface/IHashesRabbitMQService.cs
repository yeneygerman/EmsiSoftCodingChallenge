using EmsiSoft.Data.Entity;
using EmsiSoft.Models;

namespace EmsiSoft.Services.Interface
{
    public interface IHashesRabbitMQService
    {
        void AddHashes(IEnumerable<Hashes> hashList);
    }
}
