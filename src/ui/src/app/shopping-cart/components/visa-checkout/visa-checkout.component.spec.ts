import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisaCheckoutComponent } from './visa-checkout.component';

describe('VisaCheckoutComponent', () => {
  let component: VisaCheckoutComponent;
  let fixture: ComponentFixture<VisaCheckoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VisaCheckoutComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VisaCheckoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
