import {Injectable} from "@angular/core";
import {AuthService} from "../authentication/services/authentication.service";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: "root",
})
export class CustomerService {
  constructor(
    private authService: AuthService,
    private http: HttpClient,
  ) {}

  get baseUrl() {
    if (!this.authService.isAuthenticated) throw new Error();

    const userId = this.authService.getAuthenticatedUser()?.id;

    return `${environment.customersBaseUrl}/customers/${userId}`;
  }

  getMyProfile() {
    return this.http.get<any>(`${this.baseUrl}`);
  }
}
