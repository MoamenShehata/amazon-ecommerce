import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { CartItemCreateModel } from './models/cart-item-create.models';
import { AuthService } from '../authentication/services/authentication.service';
import { StorageService } from '../core/services/storage-service';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
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

  addCartItem(cartItem: CartItemCreateModel) {
    if (!this.activeCartId) {
      return this.initCart(cartItem);
    }

    return this.http.post<any>(this.cartItemsBaseUrl, cartItem);
  }

  private initCart(cartItem: CartItemCreateModel) {
    return this.http
      .post<any>(this.cartsBaseUrl, {
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
