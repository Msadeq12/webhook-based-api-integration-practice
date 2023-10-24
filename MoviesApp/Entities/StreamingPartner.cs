
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Entities
{
    public class StreamingPartner
    {
        public int StreamingPartnerId { get; set; }

        [Required(ErrorMessage = "Please enter the URL for the streaming partner.")]
        public string? Url { get; set; }

        public string? ApiKey { get; set; } = null;

    }
}
