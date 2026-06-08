import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ShoppingCartService } from "../../shopping-cart.services";
import { CommonModule } from "@angular/common";
import { CartItemDto } from "../../models/cart-item-model";
import { ShoppingCartState } from "../../shopping-cart.state";
import { catchError } from "rxjs";

@Component({
  selector: "product-cart-control",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./product-cart-control.component.html",
  styleUrl: "./product-cart-control.component.css",
})
export class ProductCartControlComponent {
  @Input() cartItem: CartItemDto;
  @Input() productItemIds: number[] = [];

  @Output() onAllItemsRemoved: EventEmitter<void> = new EventEmitter();

  get quantity(): number {
    return this.productItemIds?.length ?? 0;
  }

  constructor(
    private shoppingCartService: ShoppingCartService,
    private shoppingCartState: ShoppingCartState,
  ) { }

  addToCart() {
    this.shoppingCartService
      .addCartItem({
        productId: this.cartItem.productId,
      })
      .pipe(
        catchError((err) => {
          return [];
        }),
      )
      ?.subscribe((res) => {
        this.productItemIds.push(res.cartItemId);

        this.shoppingCartState.add({
          productId: this.cartItem.productId,
          productName: this.cartItem.productName,
          productImageUrl: this.cartItem.productImageUrl,
          itemIds: this.productItemIds,
          unitPrice: this.cartItem.unitPrice,
          isAvailable: this.cartItem.isAvailable
        });
      });
  }

  removeProductItem() {
    if (this.productItemIds.length === 0) {
      return;
    }

    this.shoppingCartService.removeCartItem(this.cartItem.productId)?.subscribe(() => {
      this.productItemIds = this.productItemIds.slice(0, -1);
      this.shoppingCartState.remove(this.cartItem.productId);
    });
  }

  deleteAllItemsForProduct() {
    if (this.quantity === 0) {
      return;
    }

    this.shoppingCartService
      .RemoveAllProductItems(this.cartItem.productId)
      ?.subscribe(() => {
        this.productItemIds = [];
        this.shoppingCartState.removeAll(this.cartItem.productId);
        this.onAllItemsRemoved.emit();
      });
  }
}
