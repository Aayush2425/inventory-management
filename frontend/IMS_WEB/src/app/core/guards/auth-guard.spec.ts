import { TestBed } from '@angular/core/testing';
import { AuthGuard } from './auth-guard';
import { Router, ActivatedRouteSnapshot, provideRouter } from '@angular/router';
import { vi } from 'vitest';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let router: Router;
  let route: ActivatedRouteSnapshot;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthGuard, provideRouter([])],
    });

    guard = TestBed.inject(AuthGuard);
    router = TestBed.inject(Router);
    route = new ActivatedRouteSnapshot();
  });

  it('should redirect to /login if no token', () => {
    localStorage.removeItem('token');

    const navigateSpy = vi.spyOn(router, 'navigate');

    const result = TestBed.runInInjectionContext(() => guard.canActivate(route));

    expect(result).toBe(false);
    expect(navigateSpy).toHaveBeenCalledWith(['/login']);
  });

  it('should allow access when token exists and no roles are required', () => {
    localStorage.setItem('token', 'abc');
    route.data = {};

    const result = TestBed.runInInjectionContext(() => guard.canActivate(route));

    expect(result).toBe(true);
  });

  it('should block access and redirect when roles do not match', () => {
    localStorage.setItem('token', 'abc');
    localStorage.setItem('role', 'user');
    route.data = { roles: ['admin'] };

    const navigateSpy = vi.spyOn(router, 'navigate');

    const result = TestBed.runInInjectionContext(() => guard.canActivate(route));

    expect(result).toBe(false);
    expect(navigateSpy).toHaveBeenCalledWith(['/login']);
  });

  it('should allow access when roles match', () => {
    localStorage.setItem('token', 'abc');
    localStorage.setItem('role', 'admin');
    route.data = { roles: ['admin'] };

    const result = TestBed.runInInjectionContext(() => guard.canActivate(route));

    expect(result).toBe(true);
  });
});
