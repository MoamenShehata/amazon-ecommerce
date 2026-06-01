import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerPaymentCardsComponent } from './customer-payment-cards.component';

describe('CustomerPaymentCardsComponent', () => {
  let component: CustomerPaymentCardsComponent;
  let fixture: ComponentFixture<CustomerPaymentCardsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerPaymentCardsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CustomerPaymentCardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
