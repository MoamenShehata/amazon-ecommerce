import { Component, Input } from '@angular/core';
import { OrderDetailsDto } from '../../../orders/models/OrderForListDto';
import { NgIf } from '@angular/common';
import { BootstrapModalComponent } from '../../../core/components/bootstrap-modal/bootstrap-modal.component';
import { OrderShipStartedFormComponent } from '../order-ship-started-form/order-ship-started-form.component';
import { ModalTriggerDirective } from '../../../core/components/directives/modal-trigger.directive';
import { OrderShipedFormComponent } from '../order-shiped-form/order-shiped-form.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OrdersService } from '../../../orders/orders.services';
import { AppServicesProvider } from '../../../core/services/app-services.provider';

@Component({
  selector: 'order-timeline',
  standalone: true,
  imports: [NgIf, BootstrapModalComponent, OrderShipStartedFormComponent, OrderShipedFormComponent, ModalTriggerDirective],
  templateUrl: './order-timeline.component.html',
  styleUrl: './order-timeline.component.css'
})
export class OrderTimelineComponent extends AppServicesProvider {
  @Input() order: OrderDetailsDto;

  form: FormGroup;

  constructor(private fb: FormBuilder,
    private orderService: OrdersService
  ) {
    super();
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      to: [this.order.statusId + 1],
      payload: [, [Validators.required]]
    });
  }

  updateStatus() {
    if (this.form.invalid) {
      this.toastError('Please make sure you input all required fields correctly!')
      return;
    }

    this.orderService.updateStatus(this.order.id, this.form.value)
      .subscribe((res) => {
        this.toastSuccess('Updated successfully');

        this.order.id = this.form.value.to;

        this.closeModal();
      })
  }

  onStatusFormSubmitted(payload: any) {
    this.form.patchValue({ payload: payload });

    this.updateStatus();
  }

  showModal = true;

  closeModal() {
    this.showModal = false;
  }
}
