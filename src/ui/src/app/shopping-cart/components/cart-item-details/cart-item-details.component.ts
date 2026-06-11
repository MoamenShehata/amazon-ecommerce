import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppServicesProvider } from '../../../core/services/app-services.provider';
import { CartItemDto } from '../../models/cart-item-model';
import { ProductCartControlComponent } from '../product-cart-control/product-cart-control.component';

@Component({
  selector: 'cart-item-details',
  standalone: true,
  imports: [CommonModule, ProductCartControlComponent],
  templateUrl: './cart-item-details.component.html',
  styleUrl: './cart-item-details.component.css'
})
export class CartItemDetailsComponent extends AppServicesProvider {

  @Input() cartItem: CartItemDto;

  @Output() onAllItemsRemoved: EventEmitter<void> = new EventEmitter();

  emitOnAllItemsRemoved() {
    this.onAllItemsRemoved.emit();
  }
  get quantity(): number {
    return this.cartItem?.quantity;
  }

  get unitPrice(): number | null {
    return this.cartItem?.unitPrice ?? null;
  }

  get subtotal(): number {
    const price = this.unitPrice ?? 0;
    const result = price * this.quantity;

    this.cartItem.subTotal = result;
    // this.subTotalChange.emit(result);
    return result;
  }

}
