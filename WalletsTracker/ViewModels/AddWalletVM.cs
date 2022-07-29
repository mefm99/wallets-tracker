using System.ComponentModel.DataAnnotations;

namespace WalletsTracker.ViewModels
{
    public class AddWalletVM
    {
        [Required(ErrorMessage = "*مطلوب")]
        [RegularExpression("(01)[0-5]{1}[0-9]{8}", ErrorMessage = "*ادخل القيمة بطريقة صحيحة")]
        [StringLength(11, ErrorMessage = "*11 رقم فقط")]
        public string NumberPhone { get; set; }
        [Required(ErrorMessage = "*مطلوب")]
        [MaxLength(50, ErrorMessage = "*القيمة طويلة جداً"), MinLength(6, ErrorMessage = "*القيمة قصيرة جداً")]
        public string Title { get; set; }
        [Required(ErrorMessage = "*مطلوب")]
        [RegularExpression("^[0-9]*$", ErrorMessage = " *ادخل القيمة بطريقة صحيحة")]
        public string Balance { get; set; }
        public int UserID { get; set; }
    }
}
