import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormArray,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { CatalogService } from '../../services/catalog.services';
import {
  ProductCreateRequest,
  ProductProperty,
} from '../../models/product-create.model';

@Component({
  selector: 'product-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-create.component.html',
  styleUrls: ['./product-create.component.css'],
})
export class ProductCreateComponent implements OnInit {
  createForm!: FormGroup;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private formBuilder: FormBuilder,
    private catalogService: CatalogService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.createForm = this.formBuilder.group({
      categoryId: ['', [Validators.required]],
      name: ['', [Validators.required, Validators.minLength(3)]],
      inStockCount: [0, [Validators.required, Validators.min(0)]],
      price: [0, [Validators.required, Validators.min(0)]],
      minimumPrice: [0, [Validators.required, Validators.min(0)]],
      maximumPrice: [0, [Validators.required, Validators.min(0)]],
      properties: this.formBuilder.array([]),
    });
  }

  get propertiesArray(): FormArray {
    return this.createForm.get('properties') as FormArray;
  }

  addProperty(): void {
    const propertyGroup = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(1)]],
      value: ['', [Validators.required, Validators.minLength(1)]],
    });
    this.propertiesArray.push(propertyGroup);
  }

  removeProperty(index: number): void {
    this.propertiesArray.removeAt(index);
  }

  onPropertyChange(
    index: number,
    field: 'name' | 'value',
    value: string,
  ): void {
    const control = this.propertiesArray.at(index).get(field);
    if (control) {
      control.setValue(value);
    }
  }

  onSubmit(): void {
    if (this.createForm.invalid) {
      this.errorMessage = 'Please fill in all required fields correctly.';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    const formValue = this.createForm.value;
    const productRequest: ProductCreateRequest = {
      categoryId: formValue.categoryId,
      name: formValue.name,
      inStockCount: formValue.inStockCount,
      price: formValue.price,
      minimumPrice: formValue.minimumPrice,
      maximumPrice: formValue.maximumPrice,
      properties: formValue.properties,
    };

    this.catalogService.createProduct(productRequest).subscribe({
      next: () => {
        this.successMessage = 'Product created successfully!';
        this.isSubmitting = false;
        setTimeout(() => {
          this.router.navigate(['/catalog/products']);
        }, 1500);
      },
      error: (err) => {
        this.errorMessage =
          err.error?.message || 'Failed to create product. Please try again.';
        this.isSubmitting = false;
      },
    });
  }

  resetForm(): void {
    this.createForm.reset();
    this.propertiesArray.clear();
    this.errorMessage = '';
    this.successMessage = '';
  }
}
