import { TestBed } from '@angular/core/testing';
import { Auth } from './auth';
import { ApiService } from '../../../shared/api.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { of, throwError } from 'rxjs';
import { vi } from 'vitest';
import { AuthResponse, LoginRequest, RegisterRequest } from '../../../models/auth';
import { HttpErrorResponse } from '@angular/common/http';

describe('Auth Service', () => {
  let service: Auth;
  let apiService: {
    post: ReturnType<typeof vi.fn>;
  };

  beforeEach(() => {
    apiService = {
      post: vi.fn(),
    };

    TestBed.configureTestingModule({
      providers: [
        Auth,
        { provide: ApiService, useValue: apiService },
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });

    service = TestBed.inject(Auth);
  });

  it('should call API for login with correct payload', () => {
    const req: LoginRequest = {
      username: 'testuser',
      password: '123456',
    };

    const mockResponse: AuthResponse = {
      data: { accessToken: 'abc', refreshToken: 'xyz' },
    };

    apiService.post.mockReturnValue(of(mockResponse));

    service.handleLogin(req).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });

    expect(apiService.post).toHaveBeenCalledWith('Auth/login', req);
  });

  it('should call API for register with correct payload', () => {
    const req: RegisterRequest = {
      username: 'newuser',
      password: 'password',
      fullname: 'New User',
      role: 'user',
    };

    const mockResponse: AuthResponse = {
      data: { accessToken: 'new-token', refreshToken: 'new-refresh' },
    };

    apiService.post.mockReturnValue(of(mockResponse));

    service.handleRegister(req).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });

    expect(apiService.post).toHaveBeenCalledWith('Auth/register', req);
  });

  it('should return error when API fails', () => {
    const loginReq: LoginRequest = { username: 'x', password: 'y' };

    const error = new HttpErrorResponse({
      status: 401,
      statusText: 'Unauthorized',
      error: { message: 'Invalid credentials' },
    });

    apiService.post.mockReturnValue(throwError(() => error));

    service.handleLogin(loginReq).subscribe({
      error: (err) => {
        expect(err.status).toBe(401);
        expect(err.error.message).toBe('Invalid credentials');
      },
    });
  });
});
