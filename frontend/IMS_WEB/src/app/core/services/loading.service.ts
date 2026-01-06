import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  private activeRequests = signal(0);

  isLoading = signal(false);

  increaseActiveRequests(): void {
    this.activeRequests.update((count) => count + 1);
    this.isLoading.set(this.activeRequests() > 0);
  }

  decreaseActiveRequests(): void {
    this.activeRequests.update((count) => Math.max(0, count - 1));
    this.isLoading.set(this.activeRequests() > 0);
  }

  reset(): void {
    this.activeRequests.set(0);
    this.isLoading.set(false);
  }
}
