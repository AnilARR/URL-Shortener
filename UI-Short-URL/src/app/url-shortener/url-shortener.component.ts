import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-url-shortener',
  templateUrl: './url-shortener.component.html',
  styleUrls: ['./url-shortener.component.css']
})
export class UrlShortenerComponent {
  longUrl: string = ''; // Input URL from the user
  shortUrl: string | null = null; // The generated short URL
  error: string | null = null; // Error message if any
  copied: boolean = false; // Flag to track if the URL has been copied
  isLoading: boolean = false; // Loading flag to disable button during API call

  private apiUrl = environment.apiUrl + '/generate';
  private apiURLCount = ''
  constructor(private http: HttpClient) {}

  generateShortUrl() {
    if (!this.longUrl) {
      this.error = "The URL cannot be empty.";
      return;
    }

    this.isLoading = true; // Set loading to true when making the request
    this.http.post<any>(this.apiUrl, { longUrl: this.longUrl }).subscribe(
      response => {
        this.shortUrl = response.shortUrl;
        this.error = null;
        this.copied = false; // Reset copied flag when a new URL is generated
        this.isLoading = false; // Reset loading after API call completes
      },
      err => {
        this.error = "Failed to generate the short URL or Invalid URL format. Please try again.";
        this.shortUrl = null;
        this.isLoading = false; // Reset loading if there's an error
      }
    );
  }

  copyToClipboard() {
    if (this.shortUrl) {
      navigator.clipboard.writeText(this.shortUrl).then(
        () => {
          this.copied = true;
          setTimeout(() => {
            this.copied = false; // Reset after 2 seconds
          }, 9000);
        },
        () => {
          this.error = "Failed to copy the URL. Please try again.";
        }
      );
    }
  }
}
