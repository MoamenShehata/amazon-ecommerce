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
  private baseUrl = `${environment.cartBaseUrl}/carts`;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private storageService: StorageService,
  ) {}

  addCartItem(cartItem: CartItemCreateModel) {
    const activeCartId = this.storageService.retrieve('cartId');
    if (!activeCartId) {
      return this.initCart(cartItem).pipe(
        tap((resp) => {
          this.storageService.save('cartId', resp.cartId);
        }),
      );
    }

    alert('Please implement add cart api');
    return;
  }

  private initCart(cartItem: CartItemCreateModel) {
    return this.http.post<any>(this.baseUrl, {
      customerId: this.customerId,
      cartItem: cartItem,
    });
  }

  get customerId() {
    const user = this.authService.getAuthenticatedUser();
    return user?.id ?? null;
  }
}
