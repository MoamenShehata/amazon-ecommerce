import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {

  private activeRequests = 0;

  readonly loading = signal(false);

  show() {
    this.activeRequests++;

    if (this.activeRequests > 0) {
      this.loading.set(true);
    }
  }

  hide() {
    this.activeRequests--;

    if (this.activeRequests <= 0) {
      this.activeRequests = 0;
      this.loading.set(false);
    }
  }
}