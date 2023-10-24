using MoviesApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Models
{
    public class StreamingPartnerModel
    {
        [Required(ErrorMessage = "Please enter partner URL")]
        public string Url { get; set; }

    }
}
