using System.ComponentModel.DataAnnotations;

namespace WalletsTracker.ViewModels
{
    public class WithdrawalVM
    {
        public int Id { get; set; }
        public string NumberPhone { get; set; }
        [Required(ErrorMessage = "*مطلوب")]
        [RegularExpression("^[0-9]*$", ErrorMessage = " *ادخل القيمة بطريقة صحيحة")]
        public string Balance { get; set; }
    }
}
