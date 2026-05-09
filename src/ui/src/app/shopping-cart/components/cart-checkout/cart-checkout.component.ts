import { Component } from '@angular/core';
import { ShoppingCartComponent } from '../shopping-cart/shopping-cart.component';

@Component({
  selector: 'cart-checkout',
  standalone: true,
  imports: [ShoppingCartComponent],
  templateUrl: './cart-checkout.component.html',
  styleUrl: './cart-checkout.component.css',
})
export class CartCheckoutComponent {}
