import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { AppServicesProvider } from '../../../core/services/app-services.provider';
import { NgIf } from '@angular/common';

@Component({
  selector: 'order-shiped-form',
  standalone: true,
  imports: [NgIf, ReactiveFormsModule],
  templateUrl: './order-shiped-form.component.html',
})
export class OrderShipedFormComponent extends AppServicesProvider {
  @Output() submitted = new EventEmitter<any>();

  form: FormGroup;

  constructor(private fb: FormBuilder
  ) {
    super();
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      trackingId: ['', [Validators.required]],
    });
  }

  get trackingId() {
    return this.form?.get('trackingId')!;
  }

  onSubmit() {
    if (this.form.invalid) {
      this.toastError('Please make sure you input all required fields correctly!')
      return;
    }

    this.submitted.emit(this.trackingId.value);
  }
}
