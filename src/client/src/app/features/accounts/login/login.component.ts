import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountsService } from '../../../core/services/accounts.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatFormField,
    MatInput,
    MatLabel,
    MatButton,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private accountsService = inject(AccountsService);
  private router = inject(Router);

  loginForm = this.fb.group({
    email: this.fb.control<string | null>(null, [Validators.required]),
    password: this.fb.control<string | null>(null, [Validators.required]),
  });

  onSubmit() {
    const formValue = this.loginForm.getRawValue();
    console.log({ formValue });
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.accountsService.login(this.loginForm.value).subscribe({
      next: () => {
        console.log('was able to login successfully')
        this.accountsService.getUserInfo();
        this.router.navigateByUrl('/shop');
      },
    });
  }
}
