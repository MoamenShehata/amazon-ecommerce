import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ProductForListModel } from "../../models/product-for-list-model";
import { CommonModule } from "@angular/common";
import { ShoppingCartService } from "../../../shopping-cart/shopping-cart.services";
import { StorageService } from "../../../core/services/storage-service";
import { ProductCartControlComponent } from "../../../shopping-cart/components/product-cart-control/product-cart-control.component";
import { CartItemDto } from "../../../shopping-cart/models/cart-item-model";
import { AppServicesProvider } from "../../../core/services/app-services.provider";
import { CatalogService } from "../../services/catalog.services";

@Component({
  selector: "product-preview",
  standalone: true,
  imports: [CommonModule, ProductCartControlComponent],
  templateUrl: "./product-preview.component.html",
  styleUrl: "./product-preview.component.css",
})
export class ProductPreviewComponent extends AppServicesProvider {
  @Input() product: ProductForListModel;

  @Output() deleted = new EventEmitter();

  constructor(private catalogService: CatalogService) {
    super();
  }
  get productForModel(): CartItemDto {
    return {
      productId: this.product.id,
      productName: this.product.name,
      productImageUrl: this.product.imageUrl!,
      isAvailable: this.product.isAvailable!,
      itemIds: [],
      unitPrice: this.product.unitPrice,
    };
  }

  deleteProduct() {
    this.catalogService.deleteProduct(this.product.id)
      .subscribe(() => {
        this.deleted.emit();
      })
  }
}
