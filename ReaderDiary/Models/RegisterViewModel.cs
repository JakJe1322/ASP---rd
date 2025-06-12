using System.ComponentModel.DataAnnotations;

namespace ReaderDiary.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; }
    }
}
