import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { CartItemCreateModel } from './models/cart-item-create.models';
import { AuthService } from '../authentication/services/authentication.service';
import { StorageService } from '../core/services/storage-service';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { CartItemModel } from './models/cart-item-model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ShoppingCartService {
  private cartItems: BehaviorSubject<CartItemModel[]> = new BehaviorSubject<
    CartItemModel[]
  >([]);

  public cartItemsSource = this.cartItems.asObservable();

  private cartsBaseUrl = `${environment.cartBaseUrl}/carts`;
  private get cartItemsBaseUrl() {
    return `${environment.cartBaseUrl}/carts/${this.activeCartId}/items`;
  }

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private storageService: StorageService,
  ) {}

  loadCart() {
    return this.http
      .get<CartItemModel[]>(`${this.cartsBaseUrl}/${this.activeCartId}`)
      .pipe(
        tap((items) => {
          this.pushCartItems(items);
        }),
      );
  }

  addCartItem(cartItem: CartItemCreateModel) {
    const action: (cartItem: CartItemCreateModel) => Observable<CartItemModel> =
      !this.activeCartId ? this.initCart.bind(this) : this.addItem.bind(this);

    return action(cartItem).pipe(
      tap((cartItem) => {
        this.pushCartItems([cartItem]);
      }),
    );
  }

  private pushCartItems(items: CartItemModel[]) {
    this.cartItems.next([...this.cartItems.value, ...items]);
  }

  private addItem(cartItem: CartItemCreateModel) {
    return this.http.post<any>(this.cartItemsBaseUrl, cartItem);
  }

  private initCart(cartItem: CartItemCreateModel) {
    return this.http
      .post<CartItemModel>(this.cartsBaseUrl, {
        customerId: this.customerId,
        cartItem: cartItem,
      })
      .pipe(
        tap((resp) => {
          this.storageService.save('cartId', resp.cartId);
        }),
      );
  }

  get activeCartId() {
    return this.storageService.retrieve('cartId');
  }

  get customerId() {
    const user = this.authService.getAuthenticatedUser();
    return user?.id ?? null;
  }
}
