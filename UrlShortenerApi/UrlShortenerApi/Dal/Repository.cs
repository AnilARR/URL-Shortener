using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using UrlShortenerApi.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;
using System.Data;

namespace UrlShortenerApi.Dal
{
    public class Repository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Repository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("UrlShortenerDb");
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            return baseUrl;
        }

        public async Task<string> AddUrlAsync(string longUrl, string baseUrl)
        {
            try
            {
                urlresponse res = new urlresponse();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var shortUrl = Guid.NewGuid().ToString().Substring(0, 6);
                    var finalshortURL = $"{baseUrl}/{shortUrl}";

                    var query = "INSERT INTO Urls (LongUrl, ShortUrl, ClickCount,shortUrlCode) VALUES (@LongUrl, @ShortUrl, 0,@shortUrlCode);";
                    await connection.ExecuteAsync(query, new { LongUrl = longUrl, ShortUrl = finalshortURL, shortUrlCode = shortUrl});

                    return finalshortURL;
                }
            }
            catch (SqlException sqlEx)
            {
                // Log SQL exception
                Console.Error.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception("A database error occurred while adding the URL.", sqlEx);
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.Error.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while adding the URL.", ex);
            }
        }

        public async Task<string> GetOriginalUrlAsync(string shortenedUrl)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "SELECT LongUrl FROM Urls WHERE shortUrlCode = @ShortenedUrl";
                    var result = await connection.QuerySingleOrDefaultAsync<string>(query, new { ShortenedUrl = shortenedUrl });

                    return result;
                }
            }
            catch (SqlException sqlEx)
            {
                // Log SQL exception
                Console.Error.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception("A database error occurred while retrieving the original URL.", sqlEx);
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.Error.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while retrieving the original URL.", ex);
            }
        }

        private string GenerateShortenedUrl(string originalUrl)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    byte[] inputBytes = Encoding.ASCII.GetBytes(originalUrl);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    // Convert byte array to a string of hex digits
                    StringBuilder shortenedUrl = new StringBuilder();
                    foreach (var b in hashBytes)
                    {
                        shortenedUrl.Append(b.ToString("x2"));
                    }
                    string a = $"{GetBaseUrl()}/{shortenedUrl.ToString().Substring(0, 6)}";
                    // Take the first 6 characters as the short version
                    return a;
                }
            }
            catch (Exception ex)
            {
                // Log any errors in generating the shortened URL
                Console.Error.WriteLine($"Error in GenerateShortenedUrl: {ex.Message}");
                throw new Exception("An error occurred while generating the shortened URL.", ex);
            }
        }

        public async Task IncrementClickCountAsync(string shortenedUrl)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    string updateQuery = "UPDATE Urls SET ClickCount = ClickCount + 1 WHERE shortUrlCode = @ShortenedUrl";
                    await dbConnection.ExecuteAsync(updateQuery, new { ShortenedUrl = shortenedUrl });
                }
            }
            catch (SqlException sqlEx)
            {
                // Log SQL exception
                Console.Error.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception("A database error occurred while updating the click count.", sqlEx);
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.Error.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while updating the click count.", ex);
            }
        }

        public async Task<urlresponse> GetUrlByShortenedUrlAsync(string shortenedUrl)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    string selectQuery = "SELECT ShortUrl,ClickCount FROM Urls WHERE ShortUrl = @ShortenedUrl";
                    var url = await dbConnection.QueryFirstOrDefaultAsync<urlresponse>(selectQuery, new { ShortenedUrl = shortenedUrl });
                    return url;
                }
            }
            catch (SqlException sqlEx)
            {
                // Log SQL exception
                Console.Error.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception("A database error occurred while retrieving the URL by shortened URL.", sqlEx);
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.Error.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while retrieving the URL by shortened URL.", ex);
            }
        }
    }
}
