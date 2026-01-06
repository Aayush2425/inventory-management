import { Component, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../../../core/services/notification.service';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="notification-container">
      @for (notification of notificationService.notifications(); track notification.id) {
      <div class="alert alert-{{ notification.type }} alert-dismissible fade show" role="alert">
        {{ notification.message }}
        <button
          type="button"
          class="btn-close"
          (click)="notificationService.remove(notification.id)"
          aria-label="Close"
        ></button>
      </div>
      }
    </div>
  `,
  styles: [
    `
      .notification-container {
        position: fixed;
        top: 20px;
        right: 20px;
        z-index: 9998;
        max-width: 400px;
      }
      .alert {
        margin-bottom: 10px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        animation: slideIn 0.3s ease-out;
      }
      @keyframes slideIn {
        from {
          transform: translateX(400px);
          opacity: 0;
        }
        to {
          transform: translateX(0);
          opacity: 1;
        }
      }
      .alert-success {
        background-color: #d4edda;
        border-color: #c3e6cb;
        color: #155724;
      }
      .alert-error {
        background-color: #f8d7da;
        border-color: #f5c6cb;
        color: #721c24;
      }
      .alert-warning {
        background-color: #fff3cd;
        border-color: #ffeaa7;
        color: #856404;
      }
      .alert-info {
        background-color: #d1ecf1;
        border-color: #bee5eb;
        color: #0c5460;
      }
    `,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NotificationComponent {
  notificationService = inject(NotificationService);
}
