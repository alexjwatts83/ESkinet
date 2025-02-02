import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [MatButton],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.scss',
})
export class TestErrorComponent {
  private _baseUrl = environment.apiUrl;
  private httpClient = inject(HttpClient);

  get404Error() {
    this.httpClient.get(`${this._baseUrl}/buggy/not-found`).subscribe({
      next: (response) => console.log('here'),
    });
  }

  get400Error() {
    this.httpClient.get(`${this._baseUrl}/buggy/bad-request`).subscribe({
      next: (response) => console.log('here'),
    });
  }

  get401Error() {
    this.httpClient.get(`${this._baseUrl}/buggy/unauthorised`).subscribe({
      next: (response) => console.log('here'),
    });
  }

  get500Error() {
    this.httpClient.get(`${this._baseUrl}/buggy/internal-error`).subscribe({
      next: (response) => console.log('here'),
    });
  }

  get400ValidationError1() {
    this.httpClient.post(`${this._baseUrl}/buggy/validation-error`, {}).subscribe({
      next: (response) => console.log('here'),
    });
  }

  get400ValidationError2() {
    const productJson = {
      name: 'sajdkjasdklajsdlkasjdlkasjdaslkjdaskljdaskldjaslkdjaskldjaslkdjasdkljasdlkasj',
      description: '',
      price: -1,
      pictureUrl: '',
      type: '',
      brand: '',
      quantityInStock: -1,
    };
    this.httpClient
      .post(`${this._baseUrl}/products/`, { product: productJson })
      .subscribe({
        next: (response) => console.log('here'),
      });
  }
}
