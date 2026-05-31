import { Component, Input } from "@angular/core";
import { ShoppingCartService } from "../../shopping-cart.services";
import { CartItemModel, CartItemDto } from "../../models/cart-item-model";
import { CommonModule } from "@angular/common";
import { RouterLink } from "@angular/router";
import { ShoppingCartState } from "../../shopping-cart.state";
import { CartItemDetailsComponent } from "../cart-item-details/cart-item-details.component";

@Component({
  selector: "shopping-cart",
  standalone: true,
  imports: [CommonModule, RouterLink, CartItemDetailsComponent],
  templateUrl: "./shopping-cart.component.html",
  styleUrl: "./shopping-cart.component.css",
})
export class ShoppingCartComponent {
  cartItems: CartItemDto[] = [];

  constructor(
    private shoppingCartService: ShoppingCartService,
    private shoppingCartState: ShoppingCartState,
  ) {
    shoppingCartState._source.subscribe((cartItems) => {
      this.cartItems = cartItems;
    });
  }

  @Input() style: "full" | "mini" | "checkout" = "full";
  get isFullStyle() {
    return this.style == "full" || this.style == "checkout";
  }

  ngOnInit() {
    this.shoppingCartService.getCart().subscribe((cartItems) => {
      cartItems.forEach((cartItem) => {
        this.shoppingCartState.add(cartItem);
      });
    });
  }

  removeProductFromCart(cartItem: CartItemDto) {
    this.cartItems = this.cartItems.filter((i) => i.productId !== cartItem.productId);
  }

  get totalPrice() {
    let total = 0;
    this.cartItems.forEach((i) => total += i.subTotal ?? 0);
    return total;
  }

  get cartItemsFalttenedCount() {
    let count = 0;
    this.cartItems.forEach((i) => (count += i.itemIds.length));
    return count;
  }
}
