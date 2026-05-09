import { Component, Input } from '@angular/core';
import { ShoppingCartService } from '../../shopping-cart.services';
import { CartItemModel, CartProductDto } from '../../models/cart-item-model';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ProductCartControlComponent } from '../product-cart-control/product-cart-control.component';
import { ShoppingCartState } from '../../shopping-cart.state';

@Component({
  selector: 'shopping-cart',
  standalone: true,
  imports: [CommonModule, RouterLink, ProductCartControlComponent],
  templateUrl: './shopping-cart.component.html',
  styleUrl: './shopping-cart.component.css',
})
export class ShoppingCartComponent {
  cartItems: CartProductDto[] = [];

  constructor(
    private shoppingCartService: ShoppingCartService,
    private shoppingCartState: ShoppingCartState,
  ) {
    shoppingCartState._source.subscribe((cartItems) => {
      this.cartItems = cartItems;
    });
  }

  @Input() style: 'full' | 'mini' | 'checkout' = 'full';
  get isFullStyle() {
    return this.style == 'full' || this.style == 'checkout';
  }

  ngOnInit() {
    this.shoppingCartService.getCart().subscribe((cartItems) => {
      cartItems.forEach((cartItem) => {
        this.shoppingCartState.add(cartItem);
      });
    });
  }

  get cartItemsFalttenedCount() {
    let count = 0;
    this.cartItems.forEach((i) => (count += i.itemIds.length));
    return count;
  }
}
