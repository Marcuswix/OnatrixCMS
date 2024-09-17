using System.ComponentModel.DataAnnotations;

namespace OnatrixCMS.Model
{
    public class HelpYouFormModel
    {
        [Required(ErrorMessage = "An email is required")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
