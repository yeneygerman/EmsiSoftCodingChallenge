namespace EmsiSoft.Models
{
    public class HashResponseModel
    {
        public IList<HashGroupModel> Hashes { get; set; }
    }

    public class HashGroupModel
    {
        public DateTime Date { get; set; }

        public long Count { get; set; }
    }

    public class HashModel
    {
        public DateTime Date { get; set; }

        public IEnumerable<string> Hashes { get; set; }
    }
}