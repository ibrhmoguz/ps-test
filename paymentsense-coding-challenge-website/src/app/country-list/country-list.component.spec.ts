import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { CountryListComponent } from "./country-list.component";
import { RouterTestingModule } from "@angular/router/testing";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { MatTableModule } from "@angular/material/table";
import { MatPaginatorModule } from "@angular/material/paginator";
import { HttpClientModule } from "@angular/common/http";
import { NoopAnimationsModule } from "@angular/platform-browser/animations";
import { CountryService } from "../services";
import { of } from "rxjs";

describe("CountryListComponent", () => {
  let component: CountryListComponent;
  let fixture: ComponentFixture<CountryListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        NoopAnimationsModule,
        RouterTestingModule,
        FontAwesomeModule,
        MatTableModule,
        MatPaginatorModule,
        HttpClientModule,
      ],
      declarations: [CountryListComponent],
      providers: [CountryService],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CountryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create country component", () => {
    expect(component).toBeTruthy();
  });

  it("should display loading", () => {
    // Arrange
    component.loading = true;
    fixture.detectChanges();
    expect(
      fixture.debugElement.nativeElement.querySelector("p").textContent
    ).toContain("Loading...");
  });

  it("should display no country found", () => {
    // Act
    var countryService = TestBed.get(CountryService);
    spyOn(countryService, "getCountryList").and.returnValue(
      of({
        page: 1,
        pageSize: 3,
        total: 0,
        countries: [],
      })
    );
    component.ngOnInit();

    // Assert
    fixture.detectChanges();
    expect(
      fixture.debugElement.nativeElement.querySelector("p").textContent
    ).toContain("No country found");
  });

  it("should populate country table", () => {
    // Act
    var countryService = TestBed.get(CountryService);
    spyOn(countryService, "getCountryList").and.returnValue(
      of({
        page: 1,
        pageSize: 3,
        total: 25,
        countries: [
          {
            name: "test1",
            flag: "F1",
            population: 1231,
            timezones: [],
            currencies: [],
            languages: [],
            capital: "",
          },
          {
            name: "test2",
            flag: "F2",
            population: 1232,
            timezones: [],
            currencies: [],
            languages: [],
            capital: "",
          },
          {
            name: "test3",
            flag: "F3",
            population: 1233,
            timezones: [],
            currencies: [],
            languages: [],
            capital: "",
          },
        ],
      })
    );
    component.ngOnInit();

    // Assert
    fixture.detectChanges();

    let tableRows = fixture.nativeElement.querySelectorAll("tr");
    expect(tableRows.length).toBe(6);
  });
});
