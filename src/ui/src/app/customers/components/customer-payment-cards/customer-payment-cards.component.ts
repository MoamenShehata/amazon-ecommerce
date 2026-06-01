import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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

  @Input() style: "select" | "list" = "list";

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

  ngOnInit() {
  }


  onCardSaved(cardRequest: { cardHolder: string; cardNumber: string; expiresAt: Date }) {
    this.customerService.createPaymentCard(cardRequest).subscribe({
      next: (createdCard) => {
        this.paymentCards = [createdCard, ...this.paymentCards];
        this.toastSuccess('Payment card saved successfully.');
        this.closeCreateModal()
      },
      error: (err) => {
        this.closeCreateModal();
        this.toastError(err.error);
      },
    });
  }


  @Output() onPaymentCardSelected = new EventEmitter<number>();
  emitSelectedCard(event: any) {
    const cardId = parseInt(event.target.value);
    this.onPaymentCardSelected.emit(cardId);
  }
}
