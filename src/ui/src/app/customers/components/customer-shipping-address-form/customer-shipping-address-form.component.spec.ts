import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerShippingAddressFormComponent } from './customer-shipping-address-form.component';

describe('CustomerShippingAddressFormComponent', () => {
  let component: CustomerShippingAddressFormComponent;
  let fixture: ComponentFixture<CustomerShippingAddressFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerShippingAddressFormComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CustomerShippingAddressFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
