using System.ComponentModel.DataAnnotations;

namespace OnatrixCMS.Model
{
    public class QuestionFormModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Question {  get; set; } = null!;
    }
}
