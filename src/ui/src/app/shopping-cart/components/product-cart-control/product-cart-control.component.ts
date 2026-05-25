import {Component, Input} from "@angular/core";
import {ShoppingCartService} from "../../shopping-cart.services";
import {ProductForListModel} from "../../../poduct-catalog/models/product-for-list-model";
import {CommonModule} from "@angular/common";
import {CartItemDto} from "../../models/cart-item-model";
import {ShoppingCartState} from "../../shopping-cart.state";
import {catchError} from "rxjs";

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

  constructor(
    private shoppingCartService: ShoppingCartService,
    private shoppingCartState: ShoppingCartState,
  ) {}

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
        });
      });
  }

  removeProductItem() {
    let itemIdToRemove = this.productItemIds[this.productItemIds.length - 1];

    this.shoppingCartService
      .removeCartItem(itemIdToRemove)
      ?.subscribe((res) => {
        this.productItemIds.pop();
        this.shoppingCartState.remove(itemIdToRemove);
      });
  }

  deleteAllItemsForProduct() {
    this.shoppingCartService
      .RemoveAllProductItems(this.cartItem.productId)
      ?.subscribe((res) => {
        // this.productItemIds.pop();
      });
  }
}
