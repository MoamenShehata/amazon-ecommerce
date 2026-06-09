import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AppServicesProvider } from '../../../core/services/app-services.provider';
import { OrdersService } from '../../../orders/orders.services';

@Component({
  selector: 'order-ship-started-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './order-ship-started-form.component.html',
})
export class OrderShipStartedFormComponent extends AppServicesProvider {
  @Output() submitted = new EventEmitter<any>();

  form: FormGroup;

  constructor(private fb: FormBuilder,
    private orderService: OrdersService
  ) {
    super();
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required]],
      address: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required]],
      website: ['', [Validators.required, Validators.pattern(/^(https?:\/\/)?([\w-]+\.)+[\w-]{2,}(\/.*)?$/i)]],
    });
  }

  get name() {
    return this.form?.get('name')!;
  }

  get address() {
    return this.form?.get('address')!;
  }

  get phoneNumber() {
    return this.form?.get('phoneNumber')!;
  }

  get website() {
    return this.form?.get('website')!;
  }

  onSubmit() {
    if (this.form.invalid) {
      this.toastError('Please make sure you input all required fields correctly!')
      return;
    }

    this.submitted.emit(this.form.value);

  }
}
