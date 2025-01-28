import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { AccountsService } from '../../../core/services/accounts.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TextInputComponent } from '../../../shared/components/text-input/text-input.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, MatCard, MatButton, TextInputComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private accountsService = inject(AccountsService);
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  returnUrl = '/shop';

  loginForm = this.fb.group({
    email: this.fb.control<string | null>(null, [Validators.required]),
    password: this.fb.control<string | null>(null, [Validators.required]),
  });

  constructor() {
    const url = this.activatedRoute.snapshot.queryParams['returnUrl'];
    console.log({url});
    if (url)
      this.returnUrl = url;
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.accountsService.login(this.loginForm.value).subscribe({
      next: () => {
        this.accountsService.getUserInfo().subscribe();
        this.router.navigateByUrl(this.returnUrl);
      },
    });
  }
}
