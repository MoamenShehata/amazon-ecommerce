import { Component, Input } from '@angular/core';
import { ShoppingCartService } from '../../shopping-cart.services';
import { CartItemModel, CartProductDto } from '../../models/cart-item-model';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ProductCartControlComponent } from '../product-cart-control/product-cart-control.component';

@Component({
  selector: 'shopping-cart',
  standalone: true,
  imports: [CommonModule, RouterLink, ProductCartControlComponent],
  templateUrl: './shopping-cart.component.html',
  styleUrl: './shopping-cart.component.css',
})
export class ShoppingCartComponent {
  cartItems: CartProductDto[] = [];

  constructor(private shoppingCartService: ShoppingCartService) {}

  @Input() style: 'full' | 'mini' = 'full';
  get isFullStyle() {
    return this.style == 'full';
  }

  ngOnInit() {
    this.shoppingCartService.getCart().subscribe((cartItems) => {
      this.cartItems = cartItems;
    });
  }

  get cartItemsFalttenedCount() {
    let count = 0;
    this.cartItems.forEach((i) => (count += i.itemIds.length));
    return count;
  }
}
