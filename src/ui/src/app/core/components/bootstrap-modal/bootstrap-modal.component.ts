import { NgIf } from '@angular/common';
import { AfterContentInit, Component, ContentChild, Input } from '@angular/core';

@Component({
  selector: 'bootstrap-modal',
  standalone: true,
  imports: [NgIf],
  templateUrl: './bootstrap-modal.component.html',
})
export class BootstrapModalComponent implements AfterContentInit {
  @Input() title = '';
  @ContentChild('[slot="title"]', { static: false }) customTitle: any;

  @Input() showModal = false;
  hasCustomTitle = false;

  ngAfterContentInit() {
    this.hasCustomTitle = !!this.customTitle;
  }

  open() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }
}
