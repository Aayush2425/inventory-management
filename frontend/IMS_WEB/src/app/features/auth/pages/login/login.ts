import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiService } from '../../../../shared/api.service';
import { Router } from '@angular/router';
import { AuthResponse } from '../../../../models/auth';
import { Auth } from '../../services/auth';
import { decodeJwt } from '../../../../core/utils/jwt.util';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  showUserPassword = false;
  loading = false;
  errorMessage = '';
  successMessage = '';

  userForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    private api: ApiService,
    private router: Router,
    private auth: Auth
  ) {
    this.userForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(5)]],
      remember: [false],
    });
  }
  onUserLogin() {
    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      return;
    }
    this.resetMessages();
    this.loading = true;
    const { username, password } = this.userForm.value;
    this.auth.handleLogin({ username, password }).subscribe({
      next: (res: AuthResponse) => {
        this.handleSuccess(res, 'User login successful');
      },
      error: (err: any) => this.handleError(err),
    });
  }

  private handleSuccess(res: AuthResponse, message: string) {
    this.loading = false;
    this.successMessage = message;

    if (res.data.accessToken) {
      localStorage.setItem('token', res.data.accessToken);
    }
    const jwtJson = decodeJwt(res.data.accessToken);

    if (jwtJson['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] == 'Owner')
      this.router.navigate(['/OwnerDashboard']);
  }

  private handleError(err: any) {
    this.loading = false;
    this.errorMessage = err?.error?.message || 'Login failed';
  }

  private resetMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }
}
