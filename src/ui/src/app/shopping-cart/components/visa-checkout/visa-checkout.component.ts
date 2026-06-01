
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomerProfileComponent } from '../../../customers/components/customer-profile/customer-profile.component';
import { AppServicesProvider } from '../../../core/services/app-services.provider';
import { ShoppingCartService } from '../../shopping-cart.services';

@Component({
  selector: 'app-visa-checkout',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, CustomerProfileComponent],
  templateUrl: './visa-checkout.component.html',
  styleUrl: './visa-checkout.component.css'
})
export class VisaCheckoutComponent extends AppServicesProvider implements OnInit {
  paymentForm!: FormGroup;

  constructor(private fb: FormBuilder,
    private cartService: ShoppingCartService
  ) {
    super();
  }

  ngOnInit(): void {
    this.paymentForm = this.fb.group({
      paymentCardId: [null, [Validators.required]],
      cvv: ['', [Validators.required, Validators.pattern('^\\d{3}$')]],
    });
  }

  get f() {
    return this.paymentForm.controls;
  }

  isSubmitting = false;

  onSubmit(): void {
    this.isSubmitting = true;

    if (this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched();
      this.toastError('Please fix the errors in the form.');
      return;
    }

    this.cartService.checkoutUsingVisa(this.paymentForm.value).subscribe(
      (orederId) => {
        this.cartService.clearInMemoryCart();
        this.router.navigate(["/my/orders", orederId]);
      },
      (err) => {
        this.toastError(err.error);
      },);
  }

  setPaymentCard(cardId: number) {
    this.paymentForm.patchValue({ paymentCardId: cardId });
  }
}
