export class LoadingService {

  private activeRequests = 0;

  loading = false;

  show() {
    this.activeRequests++;

    if (this.activeRequests > 0) {
      this.loading = true;
    }
  }

  hide() {
    this.activeRequests--;

    if (this.activeRequests <= 0) {
      this.activeRequests = 0;
      this.loading = false;
    }
  }
}

const loadingService = new LoadingService();
export default loadingService;