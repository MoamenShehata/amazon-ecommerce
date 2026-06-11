import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { CartItemCreateModel } from "./models/cart-item-create.models";
import { AuthService } from "../authentication/services/authentication.service";
import { StorageService } from "../core/services/storage-service";
import { BehaviorSubject, catchError, map, Observable, tap } from "rxjs";
import { CartItemModel, CartItemDto, CartCreateResultDto } from "./models/cart-item-model";
import { HttpClient } from "@angular/common/http";
import { ShoppingCartState } from "./shopping-cart.state";
import { AppServicesProvider } from "../core/services/app-services.provider";

@Injectable({
  providedIn: "root",
})
export class ShoppingCartService extends AppServicesProvider {
  private cartsBaseUrl = `${environment.cartBaseUrl}/carts`;
  private get cartItemsBaseUrl() {
    return `${environment.cartBaseUrl}/carts/${this.activeCartId}/items`;
  }

  constructor(
    private http: HttpClient,
    private storageService: StorageService,
    private shoppingCartState: ShoppingCartState,
  ) {
    super();
  }

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

  ensureUserHasCartAndPushItem(cartItem: CartItemCreateModel) {
    const action: (cartItem: CartItemCreateModel) => Observable<CartItemDto> = !this
      .activeCartId
      ? this.initCart.bind(this)
      : this.pushProductItem.bind(this);

    return action(cartItem);
  }

  private initCart(cartItem: CartItemCreateModel) {

    return this.http
      .post<CartCreateResultDto>(
        this.cartsBaseUrl,
        {
          customerId: this.customerId,
          cartItem: cartItem,
        }
      )
      .pipe(
        map((resp: CartCreateResultDto) => {
          this.storageService.save("cartId", resp.cartId);

          return resp.cartItem;
        }),
      );
  }

  private pushProductItem(cartItem: CartItemCreateModel) {
    return this.http.post<CartItemDto>(this.cartItemsBaseUrl, cartItem);
  }

  popProductItem(productId: string) {
    return this.http.delete<any>(`${this.cartItemsBaseUrl}/${productId}`);
  }

  dropProductFromCart(productId: string) {
    return this.http.delete<any>(
      `${this.cartItemsBaseUrl}/RemoveAllProductItems/${productId}`,
    );
  }

  createOrderAndChallengePayment(dto: any) {
    if (!this.activeCartId) throw new Error("No active cart");

    return this.http.post<{ redirectUrl: string, paymentMehod: number }>(
      `${this.cartsBaseUrl}/${this.activeCartId}/Checkout/CreateOrder`,
      dto,
    );
  }

  confirmPayment(otp: any = null, visaDetails: any = null) {
    if (!this.activeCartId) return;

    return this.http.post<any>(
      `${this.cartsBaseUrl}/${this.activeCartId}/Checkout/ConfirmPayment`,
      { otp: otp, visaDetails: visaDetails }
    );
  }

  clearInMemoryCart() {
    this.storageService.delete("cartId");
    this.shoppingCartState.clear();
  }

  get activeCartId() {
    return this.storageService.retrieve("cartId");
  }

  get customerId() {
    const user = this.authService.getAuthenticatedUser();
    return user?.id ?? null;
  }
}
