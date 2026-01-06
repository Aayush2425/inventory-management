import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ErrorService } from '../services/error.service';
import { NotificationService } from '../services/notification.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const errorService = inject(ErrorService);
  const notificationService = inject(NotificationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const errorMsg = errorService.formatErrorMessage(error.status, error.error?.message);

      const appError = {
        id: Date.now().toString(),
        message: errorMsg,
        statusCode: error.status,
        timestamp: new Date(),
      };

      errorService.addError(appError);
      notificationService.error(errorMsg);

      console.error('HTTP Error:', error);

      return throwError(() => error);
    })
  );
};
