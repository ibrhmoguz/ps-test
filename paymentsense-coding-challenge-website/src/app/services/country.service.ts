import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { apiUrl } from "src/environments/environment";
import { PaginatedResult } from '../models/paginatedResult';

@Injectable({
  providedIn: "root",
})
export class CountryService {
  constructor(private httpClient: HttpClient) {}

  public getCountryList(pageInfo:any): Observable<PaginatedResult> {
    return this.httpClient
      .get(`${apiUrl.country}/list?Page=${pageInfo.page}&PageSize=${pageInfo.pageSize}`)
      .pipe(map((data: any) => data));
  }
}
