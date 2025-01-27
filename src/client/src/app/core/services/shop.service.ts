import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/products';
import { Observable } from 'rxjs';
import { ShopParams } from '../../shared/models/shop-params';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  private _baseUrl = 'https://localhost:5151/api';
  private httpClient = inject(HttpClient);

  types: string[] = [];
  brands: string[] = [];

  getProducts(shopParams: ShopParams): Observable<Pagination<Product>> {
    let params = new HttpParams();
    console.log({ shopParams });
    params = params.append('brand', shopParams.brands.join(','));
    params = params.append('type', shopParams.types.join(','));
    params = params.append('sort', shopParams.sort);
    params = params.append('search', shopParams.search);
    params = params.append('pageNumber', shopParams.pageNumber);
    params = params.append('pageSize', shopParams.pageSize);
    console.log({params});
    return this.httpClient.get<Pagination<Product>>(`${this._baseUrl}/products`, { params });
  }

  getProduct(id: string): Observable<Product> {
    console.log({id});
    return this.httpClient.get<Product>(`${this._baseUrl}/products/${id}`,);
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
