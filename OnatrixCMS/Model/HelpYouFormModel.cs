using System.ComponentModel.DataAnnotations;

namespace OnatrixCMS.Model
{
    public class HelpYouFormModel
    {
        [Required(ErrorMessage = "An email is required")]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]{2,}@[a-zA-Z0-9.-]{2,}\.[a-zA-Z]{2,}$", ErrorMessage = "This email isn't valid")]
        [Display(Name = "HelpEmail", Order = 0, Prompt = "Email Address")]
        public string HelpEmail { get; set; } = null!;

        public string Date { get; set; } = System.DateTime.Now.ToString();
    }
}
