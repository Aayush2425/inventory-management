import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { retry, catchError, throwError } from 'rxjs';

export const retryInterceptor: HttpInterceptorFn = (req, next) => {
  // Only retry GET requests and specific status codes
  const canRetry = req.method === 'GET';

  if (!canRetry) {
    return next(req);
  }

  return next(req).pipe(
    retry({
      count: 3,
      delay: (error: HttpErrorResponse) => {
        if (error.status >= 400 && error.status < 500) {
          return throwError(() => error);
        }

        const retryCount = (error as any).retry_count || 0;
        const delayMs = Math.pow(2, retryCount) * 1000;
        console.log(`Retrying request in ${delayMs}ms...`);

        return new Promise((resolve) => setTimeout(resolve, delayMs));
      },
    }),
    catchError((error) => throwError(() => error))
  );
};
