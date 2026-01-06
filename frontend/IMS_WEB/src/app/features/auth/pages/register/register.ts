import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Auth } from '../../services/auth';
import { AuthResponse } from '../../../../models/auth';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  showUserPassword = false;
  loading = false;
  errorMessage = '';
  successMessage = '';

  userForm: FormGroup;
  constructor(private fb: FormBuilder, private router: Router, private auth: Auth) {
    this.userForm = this.fb.group({
      username: ['', [Validators.required]],
      fullname: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(5)]],
      role: ['', [Validators.required]],
    });
  }
  onUserRegister() {
    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      return;
    }
    this.resetMessages();
    this.loading = true;
    const { username, password, role } = this.userForm.value;
    this.auth.handleRegister({ username, password, role }).subscribe({
      next: (res: AuthResponse) => {
        this.handleSuccess(res, 'User Created successful');
      },
      error: (err: any) => this.handleError(err),
    });
  }

  private handleSuccess(res: AuthResponse, message: string) {
    this.loading = false;
    this.successMessage = message;

    this.router.navigate(['/Login']);
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
