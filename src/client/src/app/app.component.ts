import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './layout/header/header.component';
import { HttpClient } from '@angular/common/http';
import { NgFor } from '@angular/common';
import { Product } from './shared/models/products';
import { Pagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  private httpClient = inject(HttpClient);

  title = 'client';
  baseUrl = 'https://localhost:5151/api';
  products: Product[] = [];

  constructor() {
    console.log('here');
  }

  ngOnInit(): void {
    console.log('ngOnInit');
    this.httpClient.get<Pagination<Product>>(`${this.baseUrl}/products?pageNumber=1&pageSize=10`).subscribe({
      next: x => {
        this.products = x.data;
        console.log({prod: this.products});
      },
      error: error => console.error(error),
      complete: () => console.log('completed')
    });
  }
}
