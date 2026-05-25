import {Injectable} from "@angular/core";
import {environment} from "../../environments/environment";
import {CartItemCreateModel} from "./models/cart-item-create.models";
import {AuthService} from "../authentication/services/authentication.service";
import {StorageService} from "../core/services/storage-service";
import {BehaviorSubject, catchError, map, Observable, tap} from "rxjs";
import {CartItemModel, CartItemDto} from "./models/cart-item-model";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: "root",
})
export class ShoppingCartService {
  private cartsBaseUrl = `${environment.cartBaseUrl}/carts`;
  private get cartItemsBaseUrl() {
    return `${environment.cartBaseUrl}/carts/${this.activeCartId}/items`;
  }

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private storageService: StorageService,
  ) {}

  getCart() {
    const activeCartId = this.activeCartId;
    if (!activeCartId)
      return new BehaviorSubject<CartItemDto[]>([]).asObservable();

    return this.http
      .get<CartItemDto[]>(`${this.cartsBaseUrl}/${activeCartId}`)
      .pipe(
        catchError((err) => {
          if (err.status === 404) {
            this.storageService.delete("cartId");
          }
          return new BehaviorSubject<CartItemDto[]>([]).asObservable();
        }),
      );
  }

  addCartItem(cartItem: CartItemCreateModel) {
    const action: (cartItem: CartItemCreateModel) => Observable<any> = !this
      .activeCartId
      ? this.initCart.bind(this)
      : this.addItem.bind(this);

    return action(cartItem);
  }

  private initCart(cartItem: CartItemCreateModel) {
    return this.http
      .post<CartItemModel>(this.cartsBaseUrl, {
        customerId: this.customerId,
        cartItem: cartItem,
      })
      .pipe(
        map((resp: any) => {
          this.storageService.save("cartId", resp.value.cartId);

          if (resp.message) {
            alert(resp.message);
            throw new Error(resp.message);
          }

          return resp.value;
        }),
      );
  }

  private addItem(cartItem: CartItemCreateModel) {
    return this.http.post<any>(this.cartItemsBaseUrl, cartItem);
  }

  removeCartItem(cartItemId: number) {
    return this.http.delete<any>(`${this.cartItemsBaseUrl}/${cartItemId}`);
  }

  RemoveAllProductItems(productId: string) {
    return this.http.delete<any>(
      `${this.cartItemsBaseUrl}/RemoveAllProductItems/${productId}`,
    );
  }

  checkoutUsingOtp(otp: string) {
    if (!this.activeCartId) return;

    return this.http.post<any>(
      `${this.cartsBaseUrl}/${this.activeCartId}/checkoutOtp`,
      {otp},
    );
  }

  get activeCartId() {
    return this.storageService.retrieve("cartId");
  }

  get customerId() {
    const user = this.authService.getAuthenticatedUser();
    return user?.id ?? null;
  }
}
