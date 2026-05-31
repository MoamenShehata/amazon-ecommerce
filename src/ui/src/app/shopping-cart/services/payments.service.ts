import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { AuthService } from "../../authentication/services/authentication.service";
import { PaymentMethod } from "../models/payment-method.model";
import { tap } from "rxjs";

@Injectable({
    providedIn: "root",
})
export class PaymentsService {
    private baseUrl = `${environment.cartBaseUrl}/paymentMethods`;


    constructor(
        private http: HttpClient,
        private authService: AuthService
    ) { }

    getPaymentMethods() {
        return this.http
            .get<PaymentMethod[]>(this.baseUrl);
    }


    createPaymentRequest(paymentMethodId: string, deliverToAddressId: number | null = null) {
        const options = {
            headers: { "Authorization": `Bearer ${this.authService.accessToken}` }
        };

        return this.http
            .post<any>(`${this.baseUrl}/${paymentMethodId}/requests`, { deliverToAddressId }, options);
    }
}