using System.ComponentModel.DataAnnotations;

namespace OnatrixCMS.Model
{
    public class ContactFormModel
    {
        public string FormName { get; set; } = null!;

        [Required(ErrorMessage = "You must enter a name")]
        [Display(Name = "Name", Order = 0)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "You must enter an email")]
        [EmailAddress]
        [MaxLength(100)]
        [Display(Name = "Email", Order = 0)]
        public string Email { get; set; } = null!;

        [Display(Name = "Phone", Order = 2)]
        [MaxLength(20)]
        public string? Phone { get; set; }

        [Display(Name = "Message", Order = 3)]
        public string Message { get; set; } = null!;

        public string DateTime { get; set; } = System.DateTime.Now.ToString();
    }
}
