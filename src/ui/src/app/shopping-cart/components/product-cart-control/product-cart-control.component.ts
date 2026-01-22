import { Component, Input } from '@angular/core';
import { ShoppingCartService } from '../../shopping-cart.services';
import { ProductForListModel } from '../../../poduct-catalog/models/product-for-list-model';
import { CommonModule } from '@angular/common';
import { CartProductDto } from '../../models/cart-item-model';

@Component({
  selector: 'product-cart-control',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-cart-control.component.html',
  styleUrl: './product-cart-control.component.css',
})
export class ProductCartControlComponent {
  @Input() product: CartProductDto;
  @Input() productItemIds: number[] = [];

  constructor(private shoppingCartService: ShoppingCartService) {}

  addToCart() {
    this.shoppingCartService
      .addCartItem({
        productId: this.product.productId,
        productName: this.product.productName,
        productImageUrl: this.product.productImageUrl,
      })
      ?.subscribe((res) => {
        this.productItemIds.push(res.cartItemId);
      });
  }

  removeProductItem() {
    let itemIdToRemove = this.productItemIds[this.productItemIds.length - 1];

    this.shoppingCartService
      .removeCartItem(itemIdToRemove)
      ?.subscribe((res) => {
        this.productItemIds.pop();
      });
  }

  deleteAllItemsForProduct() {}
}
