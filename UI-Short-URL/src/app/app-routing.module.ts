import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UrlShortenerComponent } from './url-shortener/url-shortener.component';
import { AnalyticsComponent } from './analytics/analytics.component'; // Ensure this is imported

const routes: Routes = [
  { path: '', component: UrlShortenerComponent }, // Default route to the URL shortener page
  { path: 'analytics', component: AnalyticsComponent }, // Dynamic route for analytics page with shortened URL code
  { path: '**', redirectTo: '' } // Wildcard route to redirect any unknown paths to the home page
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
