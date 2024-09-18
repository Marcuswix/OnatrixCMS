using System.ComponentModel.DataAnnotations;

namespace OnatrixCMS.Model
{
    public class ContactFormModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
