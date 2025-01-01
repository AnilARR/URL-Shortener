import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { UrlShortenerComponent } from './url-shortener/url-shortener.component';
import { FormsModule } from '@angular/forms';
import { AnalyticsComponent } from './analytics/analytics.component';
@NgModule({
  declarations: [
    AppComponent,
    UrlShortenerComponent,
    AnalyticsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
