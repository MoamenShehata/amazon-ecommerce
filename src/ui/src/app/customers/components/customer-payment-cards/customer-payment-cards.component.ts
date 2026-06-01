import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerService, PaymentCardDto } from '../../customer.services';
import { PaymentCardFormComponent } from '../../../core/components/payment-card-form/payment-card-form.component';
import { AppServicesProvider } from '../../../core/services/app-services.provider';

@Component({
  selector: 'customer-payment-cards',
  standalone: true,
  imports: [CommonModule, PaymentCardFormComponent],
  templateUrl: './customer-payment-cards.component.html',
  styleUrl: './customer-payment-cards.component.css'
})
export class CustomerPaymentCardsComponent extends AppServicesProvider implements OnInit {
  @Input() paymentCards: PaymentCardDto[] = [];

  showModal = false;
  openCreateModal() {
    this.showModal = true;
  }

  closeCreateModal() {
    this.showModal = false;
  }

  saveMessage = '';
  errorMessage = '';
  showCreateModal = false;

  constructor(private customerService: CustomerService) {
    super();
  }

  ngOnInit(): void {
    this.loadPaymentCards();
  }

  loadPaymentCards(): void {
    this.customerService.getPaymentCards().subscribe({
      next: (cards) => (this.paymentCards = cards || []),
      error: () => (this.paymentCards = []),
    });
  }


  onCardSaved(cardRequest: { cardHolder: string; cardNumber: string; expiresAt: Date }) {

    this.customerService.createPaymentCard(cardRequest).subscribe({
      next: (createdCard) => {
        this.paymentCards = [createdCard, ...this.paymentCards];
        this.toastSuccess('Payment card saved successfully.');
        this.closeCreateModal();
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to save payment card.';
      },
    });
  }

  maskCardNumber(cardNumber: string): string {
    const last4 = cardNumber.slice(-4);
    return `**** **** **** ${last4}`;
  }

  formatExpiry(expiresAt: string): string {
    const date = new Date(expiresAt);
    if (isNaN(date.getTime())) {
      return expiresAt;
    }
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const year = date.getFullYear();
    return `${month}/${year}`;
  }
}
