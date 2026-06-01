import { Injectable } from "@angular/core";
import { AuthService } from "../authentication/services/authentication.service";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { CustomerProfile } from "./models/customer-profile.model";

export interface PaymentCardDto {
  id: number;
  cardHolder: string;
  cardNumber: string;
  expiresAt: string;
}

@Injectable({
  providedIn: "root",
})
export class CustomerService {
  constructor(
    private authService: AuthService,
    private http: HttpClient,
  ) { }

  private baseUrl = `${environment.customersBaseUrl}/customers`;
  private meUrl = `${this.baseUrl}/me`;

  getMyProfile() {
    return this.http.get<CustomerProfile>(this.meUrl);
  }

  addShippingAddress(request: any) {
    return this.http.post<CustomerProfile>(
      `${this.baseUrl}/ShippingAddresses`,
      request,
    );
  }

  createPaymentCard(request: {
    cardHolder: string;
    cardNumber: string;
    expiresAt: Date;
  }) {
    return this.http.post<PaymentCardDto>(
      `${this.meUrl}/PaymentCards`,
      request,
    );
  }
}
