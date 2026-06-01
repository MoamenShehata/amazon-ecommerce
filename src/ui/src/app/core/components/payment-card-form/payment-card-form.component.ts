import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'payment-card-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './payment-card-form.component.html',
  styleUrl: './payment-card-form.component.css'
})
export class PaymentCardFormComponent implements OnInit {
  paymentForm!: FormGroup;
  isSubmitting = false;
  successMessage = '';
  errorMessage = '';

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.paymentForm = this.fb.group({
      cardHolder: ['', [Validators.required, Validators.minLength(2)]],
      cardNumber: ['', [Validators.required, Validators.pattern('^\\d{13,19}$')]],
      expiryMonth: ['', [Validators.required, Validators.pattern('^(0[1-9]|1[0-2])$')]],
      expiryYear: ['', [Validators.required, Validators.pattern('^\\d{4}$')]],
    });
  }

  get f() {
    return this.paymentForm.controls;
  }

  onSubmit(): void {
    this.successMessage = '';
    this.errorMessage = '';

    if (this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched();
      this.errorMessage = 'Please fix the errors in the form.';
      return;
    }

    this.isSubmitting = true;

    // Simulate payment processing
    setTimeout(() => {
      this.isSubmitting = false;
      this.successMessage = 'Payment processed successfully (demo).';
      this.paymentForm.reset();
    }, 900);
  }
}
