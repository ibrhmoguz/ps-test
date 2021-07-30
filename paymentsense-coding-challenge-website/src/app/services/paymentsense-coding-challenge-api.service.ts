import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { apiUrl } from "src/environments/environment";

@Injectable({
  providedIn: "root",
})
export class PaymentsenseCodingChallengeApiService {
  constructor(private httpClient: HttpClient) {}

  public getHealth(): Observable<string> {
    return this.httpClient.get(apiUrl.health, { responseType: "text" });
  }
}
