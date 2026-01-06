import { Injectable } from '@angular/core';
import { decodeJwt } from '../../../core/utils/jwt.util';
import { AuthResponse, LoginRequest, RegisterRequest } from '../../../models/auth';
import { ApiService } from '../../../shared/api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  constructor(private api: ApiService) {}

  handleLogin(req: LoginRequest): Observable<AuthResponse> {
    return this.api.post<AuthResponse>('Auth/login', req);
  }
  handleRegister(req: RegisterRequest): Observable<AuthResponse> {
    return this.api.post<AuthResponse>('Auth/register', req);
  }
}
