using System.ComponentModel.DataAnnotations;

namespace EmsiSoft.Data.Entity
{
    public class Hashes
    {
        [Key]
        public long ID { get; set; }

        public DateTime Date { get; set; }

        public string SHA1 { get; set; }
    }
}
