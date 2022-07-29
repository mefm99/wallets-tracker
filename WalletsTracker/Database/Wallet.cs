using System.ComponentModel.DataAnnotations.Schema;

namespace WalletsTracker.Database
{
    public class Wallet
    {
        public int Id { get; set; }
        public string NumberPhone { get; set; }
        public string Title { get; set; }
        public string Balance { get; set; }
        public bool IsDelete { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
