import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { AccountsService } from '../../../core/services/accounts.service';
import { Router } from '@angular/router';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { TextInputComponent } from "../../../shared/components/text-input/text-input.component";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatButton,
    TextInputComponent
],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private accountsService = inject(AccountsService);
  private router = inject(Router);
  private snack = inject(SnackbarService);

  validationErrors?: string[];
  registerForm = this.fb.group({
    firstName: this.fb.control<string | null>(null, [Validators.required]),
    lastName: this.fb.control<string | null>(null, [Validators.required]),
    email: this.fb.control<string | null>(null, [Validators.required]),
    password: this.fb.control<string | null>(null, [Validators.required]),
  });

  onSubmit() {
    this.validationErrors = undefined;
    const formValue = this.registerForm.getRawValue();
    console.log({ formValue });
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      let formErrors = getFormValidationErrors(this.registerForm);
      console.log({formErrors});
      return;
    }

    this.accountsService.register(this.registerForm.value).subscribe({
      next: () => {
        console.log('was able to register successfully');
        this.snack.success('Registration Succesful - you can now login');
        this.router.navigateByUrl('account/login');
      },
      error: err => {
        console.log(err);
        this.validationErrors = err;
      }
    });
  }
}

export function getFormValidationErrors(form: FormGroup) {

  const result: any[] = [];
  Object.keys(form.controls).forEach(key => {

    const controlErrors = form.get(key)?.errors;
    if (controlErrors) {
      Object.keys(controlErrors).forEach(keyError => {
        result.push({
          'control': key,
          'error': keyError,
          'value': controlErrors[keyError]
        });
      });
    }
  });

  return result;
}
