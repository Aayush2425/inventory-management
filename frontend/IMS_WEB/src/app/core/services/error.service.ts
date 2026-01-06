import { Injectable, signal } from '@angular/core';

export interface AppError {
  id: string;
  message: string;
  statusCode: number;
  timestamp: Date;
}

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  private errors = signal<AppError[]>([]);
  currentError = signal<AppError | null>(null);
  errorHistory = this.errors.asReadonly();

  addError(error: AppError): void {
    this.errors.update((e) => [...e, error]);
    this.currentError.set(error);

    // Auto-clear after 5 seconds
    setTimeout(() => this.clearError(error.id), 5000);
  }

  clearError(id: string): void {
    this.errors.update((e) => e.filter((err) => err.id !== id));
    if (this.currentError()?.id === id) {
      this.currentError.set(null);
    }
  }

  clearAll(): void {
    this.errors.set([]);
    this.currentError.set(null);
  }

  formatErrorMessage(status: number, message?: string): string {
    const statusMessages: { [key: number]: string } = {
      400: 'Bad Request - Invalid data',
      401: 'Unauthorized - Please login again',
      403: 'Forbidden - You do not have permission',
      404: 'Not Found - Resource does not exist',
      500: 'Server Error - Please try again later',
      503: 'Service Unavailable - Please try again later',
    };

    return message || statusMessages[status] || 'An error occurred';
  }
}
