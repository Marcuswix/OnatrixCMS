using System.ComponentModel.DataAnnotations;

namespace OnatrixCMS.Model
{
    public class QuestionFormModel
    {
        public string? FormName { get; set; }

        [Required(ErrorMessage = "You must enter a name")]
        [Display(Name = "Name", Order = 0, Prompt = "Name")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "You must enter an email address")]
        [EmailAddress]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "You must enter a valid email (xx@xx.xx)")]
        [MaxLength(100)]
        [Display(Name = "Email", Order = 0, Prompt = "Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "You must enter a question")]
        [Display(Name = "Message", Order = 3, Prompt = "Your question")]
        public string Message { get; set; } = null!;

        public string DateTime { get; set; } = System.DateTime.Now.ToString();
    }
}
