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
import { PageRequest } from '../../../core/models/page-request.models';
import { CategoryForListModel } from '../../models/category-for-list.models';

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
  selectedImage: File | null = null;
  imagePreviewUrl: string | null = null;

  pageRequest: PageRequest = {
    pageNumber: 1,
    pageSize: 100,
    lastSeenValue: null,
  };

  categories: CategoryForListModel[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private catalogService: CatalogService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.initializeForm();

    this.catalogService
      .getCategoriesPage(this.pageRequest)
      .subscribe((page) => {
        this.categories = page.items;
      });
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

    this.catalogService
      .createProduct(productRequest, this.selectedImage!)
      .subscribe({
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
    this.selectedImage = null;
    this.imagePreviewUrl = null;
    this.errorMessage = '';
    this.successMessage = '';
  }

  onImageSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      // Validate file type
      const validImageTypes = [
        'image/jpeg',
        'image/png',
        'image/gif',
        'image/webp',
      ];
      if (!validImageTypes.includes(file.type)) {
        this.errorMessage =
          'Please select a valid image file (JPEG, PNG, GIF, or WebP)';
        return;
      }

      // Validate file size (5MB max)
      const maxSizeInBytes = 5 * 1024 * 1024;
      if (file.size > maxSizeInBytes) {
        this.errorMessage = 'Image file size must not exceed 5MB';
        return;
      }

      this.selectedImage = file;
      this.errorMessage = '';

      // Create preview
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.imagePreviewUrl = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  removeImage(): void {
    this.selectedImage = null;
    this.imagePreviewUrl = null;
  }
}
