using System.ComponentModel.DataAnnotations;
namespace WalletsTracker.Models
{
    public class LoginVM
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Required")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
