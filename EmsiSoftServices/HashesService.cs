using Data;
using EmsiSoft.Data.Entity;
using EmsiSoft.Extension;
using EmsiSoft.Models;
using EmsiSoft.Services.Interface;

namespace EmsiSoft.Services
{
    public class HashesService : IHashesService, IHashesRabbitMQService
    {
        private readonly EmsiSoftContextDB _dbContext;

        public HashesService(EmsiSoftContextDB dbContext) {
            _dbContext = dbContext;
        }

        public IList<HashGroupModel> ListGroupedByDate()
        {
            try
            {
                return _dbContext.Hashes.GroupBy(x => x.Date).Select(g => new HashGroupModel { Date = g.Key.Date, Count = g.Count() }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddHashes(IEnumerable<Hashes> hashList)
        {
            try
            {
                _dbContext.Hashes.AddRange(hashList);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<string> GenerateRandomHash(int count)
        {
            try
            {
                var randomHashList = new List<string>();

                for (int i = 0; i < count; i++)
                {
                    randomHashList.Add(Guid.NewGuid().Hash());
                }

                return randomHashList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}