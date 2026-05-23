import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CachCheckoutComponent } from './cach-checkout.component';

describe('CachCheckoutComponent', () => {
  let component: CachCheckoutComponent;
  let fixture: ComponentFixture<CachCheckoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CachCheckoutComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CachCheckoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
