import { Injectable } from "@angular/core";
import { AuthService } from "../authentication/services/authentication.service";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { CustomerProfile } from "./models/customer-profile.model";

@Injectable({
  providedIn: "root",
})
export class CustomerService {
  constructor(
    private authService: AuthService,
    private http: HttpClient,
  ) { }

  private baseUrl = `${environment.customersBaseUrl}/customers`;

  getMyProfile() {
    return this.http.get<CustomerProfile>(`${this.baseUrl}`);
  }

  addShippingAddress(request: any) {
    return this.http.post<CustomerProfile>(
      `${this.baseUrl}/ShippingAddresses`,
      request,
    );
  }
}
