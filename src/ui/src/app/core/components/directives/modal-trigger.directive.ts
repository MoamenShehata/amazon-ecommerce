import { Directive, Input, HostListener } from "@angular/core";
import { BootstrapModalComponent } from "../bootstrap-modal/bootstrap-modal.component";

@Directive({
  selector: '[modalToTrigger]',
  standalone: true
})
export class ModalTriggerDirective {
  @Input('modalToTrigger') modalToTrigger!: BootstrapModalComponent;

  @HostListener('click')
  openModal() {
    if (this.modalToTrigger) {
      this.modalToTrigger.open();
    }
  }
}