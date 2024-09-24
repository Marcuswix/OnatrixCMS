using System.ComponentModel.DataAnnotations;

namespace OnatrixCMS.Model
{
    public class ContactFormToSendModel
    {
        public string FormName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string Message { get; set; } = null!;

        public string DateTime { get; set; } = null!;
    }
}
