import { HttpClient } from '@angular/common/http';
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
  
  getProducts() : Observable<Pagination<Product>> {
    return this.httpClient.get<Pagination<Product>>(`${this._baseUrl}/products?pageNumber=1&pageSize=10`);
  }
}
