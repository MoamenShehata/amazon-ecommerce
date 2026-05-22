import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {CountryLookup} from "../models/country-lookup.model";

@Injectable({
  providedIn: "root",
})
export class CountriesService {
  constructor(private http: HttpClient) {}

  private baseUrl = `${environment.lookupsBaseUrl}`;

  getCountries() {
    return this.http.get<CountryLookup[]>(`${this.baseUrl}/countries`);
  }
}
