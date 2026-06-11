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
  // @Input() productItemIds: number[] = [];

  @Output() onAllItemsRemoved: EventEmitter<void> = new EventEmitter();

  quantity = 0;
  ngOnInit() {
    this.quantity = this.cartItem.quantity;
  }
  constructor(
    private shoppingCartService: ShoppingCartService,
    private shoppingCartState: ShoppingCartState,
  ) { }

  addToCart() {
    this.shoppingCartService
      .ensureUserHasCartAndPushItem({
        productId: this.cartItem.productId,
      })
      .pipe(
        catchError((err) => {
          return [];
        }),
      )
      ?.subscribe((res) => {
        this.quantity = res.quantity;

        this.shoppingCartState.add(res);
      });
  }

  removeProductItem() {
    if (this.quantity == 0) return;


    this.shoppingCartService.popProductItem(this.cartItem.productId)?.subscribe(() => {
      this.quantity--;
      this.shoppingCartState.remove(this.cartItem.productId);
    });
  }

  deleteAllItemsForProduct() {
    if (this.cartItem.quantity == 0) return;


    this.shoppingCartService
      .dropProductFromCart(this.cartItem.productId)
      ?.subscribe(() => {
        this.cartItem.quantity = 0;
        this.shoppingCartState.removeAll(this.cartItem.productId);
        this.onAllItemsRemoved.emit();
      });
  }
}
