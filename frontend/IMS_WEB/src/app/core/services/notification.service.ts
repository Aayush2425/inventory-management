import { Injectable, signal } from '@angular/core';

export interface Notification {
  id: string;
  message: string;
  type: 'success' | 'error' | 'warning' | 'info';
  duration?: number;
}

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  notifications = signal<Notification[]>([]);

  show(notification: Omit<Notification, 'id'>): void {
    const id = Date.now().toString();
    const full: Notification = { ...notification, id, duration: notification.duration || 3000 };

    this.notifications.update((n) => [...n, full]);

    if (full.duration! > 0) {
      setTimeout(() => this.remove(id), full.duration);
    }
  }

  success(message: string, duration?: number): void {
    this.show({ message, type: 'success', duration });
  }

  error(message: string, duration?: number): void {
    this.show({ message, type: 'error', duration: duration || 5000 });
  }

  warning(message: string, duration?: number): void {
    this.show({ message, type: 'warning', duration });
  }

  info(message: string, duration?: number): void {
    this.show({ message, type: 'info', duration });
  }

  remove(id: string): void {
    this.notifications.update((n) => n.filter((notif) => notif.id !== id));
  }

  clear(): void {
    this.notifications.set([]);
  }
}
