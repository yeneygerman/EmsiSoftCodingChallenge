using EmsiSoft.Data.Entity;
using EmsiSoft.Models;

namespace EmsiSoft.Services.Interface
{
    public interface IHashesService
    {
        IEnumerable<string> GenerateRandomHash(int count);

        IList<HashGroupModel> ListGroupedByDate();
    }
}
