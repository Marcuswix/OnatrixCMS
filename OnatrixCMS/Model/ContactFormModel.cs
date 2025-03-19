using System.ComponentModel.DataAnnotations;

namespace OnatrixCMS.Model
{
    public class ContactFormModel
    {

        [Required(ErrorMessage = "Du måste ange ett namn")]
        [Display(Name = "Name", Order = 0, Prompt = "Name")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Du måste ange en epost")]
        [EmailAddress]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Du måste ange en giltlig (xx@xx.xx)")]
        [MaxLength(100)]
        [Display(Name = "Email", Order = 0, Prompt = "Epost")]
        public string Email { get; set; } = null!;

        [Display(Name = "Phone", Order = 2)]
        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Du måste välja ett av alternativen")]
        [Display(Name = "Message", Order = 3, Prompt = "Din fråga")]
        public string Message { get; set; } = null!;

        public string DateTime { get; set; } = System.DateTime.Now.ToString();
    }
}
