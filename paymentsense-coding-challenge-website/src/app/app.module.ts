import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import {
  PaymentsenseCodingChallengeApiService,
  CountryService,
} from "./services";
import { HttpClientModule } from "@angular/common/http";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { MatTableModule } from "@angular/material/table";
import { MatPaginatorModule } from "@angular/material/paginator";
import { CountryListComponent } from './country-list/country-list.component';

@NgModule({
  declarations: [AppComponent, CountryListComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FontAwesomeModule,
    MatTableModule,
    MatPaginatorModule,
  ],
  providers: [PaymentsenseCodingChallengeApiService, CountryService],
  bootstrap: [AppComponent],
})
export class AppModule {}
