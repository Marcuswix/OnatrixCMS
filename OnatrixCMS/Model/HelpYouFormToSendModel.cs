using System.ComponentModel.DataAnnotations;

namespace OnatrixCMS.Model
{
    public class HelpYouFormToSendModel
    {
        public string HelpEmail { get; set; } = null!;

        public string Date { get; set; } = null!;
    }
}
