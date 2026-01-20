import { Component } from '@angular/core';
import { ShoppingCartService } from '../../shopping-cart.services';
import { CartItemModel } from '../../models/cart-item-model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'shopping-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './shopping-cart.component.html',
  styleUrl: './shopping-cart.component.css',
})
export class ShoppingCartComponent {
  cartItems: CartItemModel[];
  constructor(private shoppingCartService: ShoppingCartService) {}

  ngOnInit() {
    this.shoppingCartService.cartItemsSource.subscribe((items) => {
      this.cartItems = items;
    });

    this.shoppingCartService.loadCart().subscribe();
  }
}
