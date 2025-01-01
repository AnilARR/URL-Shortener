using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }
        public int ClickCount { get; set; }
    }
    public class urlresponse
    {
        public string ShortUrl {  get; set; }
        public int ClickCount { get; set; }
    }
}
