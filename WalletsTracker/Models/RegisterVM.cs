using System.ComponentModel.DataAnnotations;

namespace WalletsTracker.Models
{
    public class RegisterVM
    {
        [MaxLength(20, ErrorMessage = "*This field is long"), MinLength(5, ErrorMessage = "*This field is short")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Required")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "*Invalid")]
        public string Username { get; set; }
        [MaxLength(20,ErrorMessage = "*This field is long"),MinLength(6,ErrorMessage = "*This field is short")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "*Not match")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Required")]
        public string ConfirmPassword { get; set; }
    }
}
