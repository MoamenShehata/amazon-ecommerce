import {Injectable} from "@angular/core";
import {AuthService} from "../authentication/services/authentication.service";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {SearchOrdersRequest} from "./models/search-orders.model";
import {OrderDetailsDto, OrderForListDto} from "./models/OrderForListDto";
import {PagedResult} from "../core/models/paged-result.models";
import {OrderItemDto} from "./models/OrderItemDto";

@Injectable({
  providedIn: "root",
})
export class OrdersService {
  constructor(
    private authService: AuthService,
    private http: HttpClient,
  ) {}

  baseUrl = `${environment.ordersBaseUrl}/orders`;

  getCustomerOrdersPage(searchRequest: SearchOrdersRequest) {
    return this.http.get<PagedResult<OrderForListDto>>(
      `${this.baseUrl}?pageNumber=${searchRequest.pageNumber}&pageSize=${searchRequest.pageSize}&lastSeenValue=${searchRequest.lastSeenValue}`,
    );
  }

  getOrderDetails(orderId: string) {
    return this.http.get<OrderDetailsDto>(`${this.baseUrl}/${orderId}`);
  }
}
