import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { Product } from '../../shared/models/products';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [MatButton],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.scss',
})
export class TestErrorComponent {
  private _baseUrl = 'https://localhost:5151/buggy';
  private httpClient = inject(HttpClient);

  get404Error() {
    this.httpClient.get(`${this._baseUrl}/not-found`).subscribe({
      next: (response) => console.log('here'),
    });
  }

  get400Error() {
    this.httpClient.get(`${this._baseUrl}/bad-request`).subscribe({
      next: (response) => console.log('here'),
      // error: (error) => {
      //   console.error(error);
      // },
    });
  }

  get401Error() {
    this.httpClient.get(`${this._baseUrl}/unauthorised`).subscribe({
      next: (response) => console.log('here'),
      // error: (error) => {
      //   console.error(error);
      // },
    });
  }

  get500Error() {
    this.httpClient.get(`${this._baseUrl}/internal-error`).subscribe({
      next: (response) => console.log('here'),
      // error: (error) => {
      //   console.error(error);
      // },
    });
  }

  get400ValidationError1() {
    this.httpClient.post(`${this._baseUrl}/validation-error`, {}).subscribe({
      next: (response) => console.log('here'),
      // error: (error) => {
      //   console.error(error);
      // },
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
      .post(`https://localhost:5151/api/products/`, { product: productJson })
      .subscribe({
        next: (response) => console.log('here'),
        // error: (error) => {
        //   console.error(error);
        // },
      });
  }
}
