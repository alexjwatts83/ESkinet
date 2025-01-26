import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/products';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  private _baseUrl = 'https://localhost:5151/api';
  private httpClient = inject(HttpClient);

  types: string[] = [];
  brands: string[] = [];

  getProducts(brands?: string[], types?: string[], sort?: string, pageNumber?: number, pageSize?: number): Observable<Pagination<Product>> {
    let params = new HttpParams();
    console.log({ brands, types, pageNumber, pageSize});
    if (brands && brands?.length > 0) {
      console.log('adding brands');
      params = params.append('brand', brands.join(','));
    }
    if (types && types?.length > 0) {
      console.log('adding types');
      params = params.append('type', types.join(','));
    }

    if (sort) {
      console.log('adding sort');
      params = params.append('sort', sort);
    }

    if (pageNumber) {
      console.log('adding pageNumber');
      params = params.append('pageNumber', pageNumber);
    }

    if (pageSize) {
      console.log('adding pageSize');
      params = params.append('pageSize', pageSize);
    }

    console.log({params});
    return this.httpClient.get<Pagination<Product>>(`${this._baseUrl}/products`, { params });
  }

  getBrands() {
    if (this.brands.length > 0)
      return;

    this.httpClient.get<string[]>(`${this._baseUrl}/brands`).subscribe({
      next: response => this.brands = response
    });
  }

  getTypes() {
    if (this.types.length > 0)
      return;

    this.httpClient.get<string[]>(`${this._baseUrl}/types`).subscribe({
      next: response => this.types = response
    });
  }
}
