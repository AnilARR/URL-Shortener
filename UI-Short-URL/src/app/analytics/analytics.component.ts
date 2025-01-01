import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';  // Import Router for navigation
import { environment } from '../../environments/environment'; // Import environment for API URL

interface Analytics {
  shortUrl: string;
  clickCount: number;
  longUrl: string;
}

@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.component.html',
  styleUrls: ['./analytics.component.css']
})
export class AnalyticsComponent implements OnInit {
  shortenedCode: string = ''; // Holds the shortened URL code from the route
  analyticsData: Analytics | null = null; // Holds fetched analytics data
  errorMessage: string | null = null; // Error messages
  isLoading: boolean = false; // Loading state

  private apiUrl = environment.apiUrl + '/analytics'; // Use environment-based API URL

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Clear any previous error messages
    this.errorMessage = null;

    // Capture the shortened URL code from the route parameters
    this.route.params.subscribe((params) => {
      this.shortenedCode = params['shortenedCode'];
      if (this.shortenedCode) {
        this.fetchAnalytics();
      } else {
        this.errorMessage = "No shortened URL code provided.";
      }
    });
  }

  fetchAnalytics(): void {
    if (!this.shortenedCode) {
      this.errorMessage = "Please enter a shortened code.";
      return;
    }

    this.isLoading = true;
    const decodedUrl = decodeURIComponent(this.shortenedCode);

    // Fetch analytics data for the given shortened URL
    this.http.get<Analytics>(`${this.apiUrl}?shortenedUrl=${decodedUrl}`).subscribe(
      (data) => {
        this.analyticsData = data;
        this.errorMessage = null;
        this.isLoading = false;
      },
      (error) => {
        console.error('Error fetching analytics data:', error); // Debugging information
        this.analyticsData = null;
        this.errorMessage = "Failed to fetch analytics data. Please check the URL code.";
        this.isLoading = false;
      }
    );
  }

  // Function to navigate back to the UrlShortenerComponent
  navigateToUrlShortener(): void {
    this.router.navigate(['/']).then((success) => {
      if (!success) {
        console.error('Navigation to UrlShortenerComponent failed.');
      }
    });
  }
}
