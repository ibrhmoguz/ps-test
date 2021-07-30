import { Component, OnInit, ViewChild } from "@angular/core";
import { Country } from "../models/country";
import { Subject, of, Subscription } from "rxjs";
import { takeUntil, tap, catchError, finalize } from "rxjs/operators";
import { MatPaginator } from "@angular/material/paginator";
import { MatTableDataSource } from "@angular/material/table";
import {
  animate,
  state,
  style,
  transition,
  trigger,
} from "@angular/animations";
import { Currency } from "../models/currency";
import { Language } from "../models/language";
import { CountryService } from "./../services/country.service";
import { PaginatedResult } from "../models/paginatedResult";

@Component({
  selector: "country-list",
  templateUrl: "./country-list.component.html",
  styleUrls: ["./country-list.component.scss"],
  animations: [
    trigger("detailExpand", [
      state("collapsed", style({ height: "0px", minHeight: "0" })),
      state("expanded", style({ height: "*" })),
      transition(
        "expanded <=> collapsed",
        animate("225ms cubic-bezier(0.4, 0.0, 0.2, 1)")
      ),
    ]),
  ],
})
export class CountryListComponent implements OnInit {
  public countryList = new MatTableDataSource<Country>();
  public paginatedResult: PaginatedResult;
  public columnsToDisplay: string[] = ["flag", "name"];
  public loading = false;
  private unsubscribe$ = new Subject<boolean>();
  expandedElement: Country | null;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(public countryService: CountryService) {}

  ngOnInit(): void {
    this.getCountryList({ page: 1, pageSize: 10 });
    console.log(this.paginatedResult);
  }

  getCountryList(pageInfo: any): void {
    this.loading = true;
    this.countryService
      .getCountryList(pageInfo)
      .pipe(
        takeUntil(this.unsubscribe$),
        tap((data: PaginatedResult) => {
          this.paginatedResult = data;
          this.countryList.data = data.countries;
        }),
        catchError((error: any) => {
          console.error(error);
          return of();
        }),
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe();
  }

  getCurrencies(currencies: Currency[]): string {
    return currencies.map((c) => c.name).join(",");
  }

  getLanguages(languages: Language[]): string {
    return languages.map((l) => l.name).join(",");
  }

  pageChanged(event: any): void {
    this.getCountryList({
      page: event.pageIndex + 1,
      pageSize: event.pageSize,
    });
  }
}
