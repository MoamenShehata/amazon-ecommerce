import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { CartItemDto } from "./models/cart-item-model";

@Injectable({
  providedIn: "root",
})
export class ShoppingCartState {
  _source = new BehaviorSubject<CartItemDto[]>([]);

  add(item: CartItemDto) {
    let currentItems = this._source.value;

    const existingCartItem = currentItems.find(
      (i) => i.productId == item.productId,
    );
    if (existingCartItem) {
      existingCartItem.quantity = item.quantity
    } else {
      currentItems.push(item);
    }

    this._source.next([...currentItems]);
  }

  // add(productId: string) {
  //   let currentItems = this._source.value;

  //   const cartProductItem = currentItems.find(
  //     (i) => i.productId == productId,
  //   );

  //   if (cartProductItem) {
  //     cartProductItem.quantity++;
  //   } else {
  //     currentItems.push({
  //       productId: productId,
  //       productName: 'string',
  //       productImageUrl: 'string',
  //       quantity: 0,
  //       isAvailable: true,
  //     });
  //   }

  //   this._source.next([...currentItems]);
  // }

  remove(productId: string) {
    let currentItems = this._source.value;

    const cartProductItem = currentItems.find(
      (i) => i.productId == productId,
    );

    if (cartProductItem) {
      cartProductItem.quantity--;
      if (cartProductItem.quantity == 0) {
        currentItems = currentItems.filter(x => x.productId != productId)
      }
    }

    this._source.next(currentItems);
  }

  removeAll(productId: string) {
    const currentItems = this._source.value.map((item) => {
      if (item.productId === productId) {
        return {
          ...item,
          itemIds: [],
        };
      }
      return item;
    });

    this._source.next(currentItems);
  }

  clear() {
    this._source.next([]);
  }

  cartItems$ = this._source.asObservable();
}
