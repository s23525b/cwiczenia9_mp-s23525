using System.ComponentModel.DataAnnotations;

namespace cwiczenia9_mp_s23525.Models.DTO
{
    public class RegisterRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [MinLength(10)]
        public string Password { get; set; }
    }
}
