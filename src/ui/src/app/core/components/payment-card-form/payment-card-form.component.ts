import { Component, OnInit, Output, EventEmitter } from '@angular/core';
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
  @Output() cardSaved = new EventEmitter<{
    cardHolder: string;
    cardNumber: string;
    expiresAt: Date;
  }>();

  paymentForm!: FormGroup;
  isSubmitting = false;
  successMessage = '';
  errorMessage = '';

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.paymentForm = this.fb.group({
      cardHolder: ['', [Validators.required, Validators.minLength(2)]],
      cardNumber: ['', [Validators.required, Validators.pattern('^\\d{16}$')]],
      expiresAt: ['', [Validators.required, Validators.pattern('^\\d{4}-\\d{2}$')]],
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

    const value = this.paymentForm.value;
    const cardRequest = {
      cardHolder: value.cardHolder,
      cardNumber: value.cardNumber,
      expiresAt: new Date(value.expiresAt),
    };

    this.cardSaved.emit(cardRequest);
  }
}
