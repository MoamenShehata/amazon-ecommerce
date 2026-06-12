import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { CountryLookup } from "../models/country-lookup.model";
import { PagedResult } from "../../core/models/paged-result.models";

@Injectable({
  providedIn: "root",
})
export class CountriesService {
  constructor(private http: HttpClient) { }

  private baseUrl = `${environment.gatewayBaseUrl}`;

  getCountries(pageNumber: number, lastSeenValue: number | null) {
    var query = `?pageNumber=${pageNumber}`;
    if (lastSeenValue)
      query += `&lastSeenValue=${lastSeenValue}`;
    return this.http.get<PagedResult<CountryLookup>>(`${this.baseUrl}/countries${query}`);
  }
}
