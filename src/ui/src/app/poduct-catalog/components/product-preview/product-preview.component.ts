import { Component, Input } from '@angular/core';
import { ProductForListModel } from '../../models/product-for-list-model';
import { CommonModule } from '@angular/common';
import { ShoppingCartService } from '../../../shopping-cart/shopping-cart.services';
import { StorageService } from '../../../core/services/storage-service';

@Component({
  selector: 'product-preview',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-preview.component.html',
  styleUrl: './product-preview.component.css',
})
export class ProductPreviewComponent {
  @Input() product: ProductForListModel;
  constructor(
    private shoppingCartService: ShoppingCartService,
    private storageService: StorageService,
  ) {}

  addToCart() {
    this.shoppingCartService
      .addCartItem({
        productId: this.product.id,
        quantity: 3,
      })
      ?.subscribe((res) => {
        alert(res.cartId);
      });
  }
}
