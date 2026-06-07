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

    const cartProductItem = currentItems.find(
      (i) => i.productId == item.productId,
    );
    if (cartProductItem) {
      currentItems = currentItems.map((i) => {
        if (i.productId == item.productId) {
          return {
            ...i,
            itemIds: [...item.itemIds],
          };
        }
        return i;
      });
    } else {
      currentItems.push(item);
    }

    this._source.next([...currentItems]);
  }

  remove(productId: string) {
    const currentItems = this._source.value;

    currentItems.forEach((i) => {
      i.itemIds = i.itemIds.slice(0, -1);
    });

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
