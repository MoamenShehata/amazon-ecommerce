import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { AuthService } from "../authentication/services/authentication.service";
import { CustomerProfile } from "../customers/models/customer-profile.model";

@Injectable({
    providedIn: "root",
})
export class AdminServices {
    constructor(
        private authService: AuthService,
        private http: HttpClient,
    ) { }
}