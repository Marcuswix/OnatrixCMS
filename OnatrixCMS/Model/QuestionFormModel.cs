using System.ComponentModel.DataAnnotations;

namespace OnatrixCMS.Model
{
    public class QuestionFormModel
    {
        public string? FormName { get; set; }

        [Required(ErrorMessage = "Du måste ange ett namn")]
        [Display(Name = "Name", Order = 0, Prompt = "Namn")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Du måste ange en epost")]
        [EmailAddress]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Du måste ange en giltlig epost (xx@xx.xx)")]
        [MaxLength(100)]
        [Display(Name = "Email", Order = 0, Prompt = "Epost adress")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Du måste skriva ett meddelande")]
        [Display(Name = "Message", Order = 3, Prompt = "Ditt meddelande")]
        public string Message { get; set; } = null!;

        public string DateTime { get; set; } = System.DateTime.Now.ToString();
    }
}
