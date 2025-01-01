using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.Models
{
    public class UrlRequest
    {
        [Url(ErrorMessage = "Invalid URL format.")]
        public string LongUrl { get; set; }
    }
}
