using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Data;
using UrlShortenerApi.Models;
using UrlShortenerApi.Dal;
using System.Web;
namespace UrlShortenerApi.Controllers
{
//    [Route("")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly ShortDbContext _context;
        private readonly Repository _urlRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UrlController(ShortDbContext context,Repository repository, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _urlRepository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateShortUrl([FromBody] UrlRequest request)
        {
            var longUrl = request.LongUrl;
            if (string.IsNullOrEmpty(longUrl))
            {
                return BadRequest(new { Message = "The long URL cannot be empty." });
            }
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var shortenedUrl = await _urlRepository.AddUrlAsync(longUrl, baseUrl);
            
            return Ok(new { ShortUrl = shortenedUrl });
        }


        [HttpGet("{shortenedUrl}")]
        public async Task<IActionResult> RedirectToOriginalUrl(string shortenedUrl)
        {
            var originalUrl = await _urlRepository.GetOriginalUrlAsync(shortenedUrl);

            if (string.IsNullOrEmpty(originalUrl))
            {
                return NotFound(new { Message = "The shortened URL does not exist." });
            }
            await _urlRepository.IncrementClickCountAsync(shortenedUrl);

            return Redirect(originalUrl);
        }

        [HttpGet("analytics")]
        public async Task<IActionResult> GetUrlAnalytics(string shortenedUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(shortenedUrl))
                {
                    return BadRequest("Shortened URL code is required.");
                }

                // Decode the URL to handle any encoding issues
                string decodedUrl = HttpUtility.UrlDecode(shortenedUrl);

                urlresponse response = await _urlRepository.GetUrlByShortenedUrlAsync(decodedUrl);

                if (response == null)
                {
                    return NotFound("The shortened URL does not exist.");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
