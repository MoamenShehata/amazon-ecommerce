import { Component, Input } from '@angular/core';
import { ProductForListModel } from '../../models/product-for-list-model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'product-preview',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-preview.component.html',
  styleUrl: './product-preview.component.css',
})
export class ProductPreviewComponent {
  @Input() product: ProductForListModel;
}
