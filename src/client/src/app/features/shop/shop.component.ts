import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/products';
import { NgFor } from '@angular/common';
import { MatCard } from '@angular/material/card';
import { ProductItemComponent } from './product-item/product-item.component';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [NgFor, MatCard, ProductItemComponent],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  private shopdService = inject(ShopService);

  products: Product[] = [];

  ngOnInit(): void {
    console.log('ngOnInit');
    this.shopdService.getProducts().subscribe({
      next: x => {
        this.products = x.data;
        console.log({prod: this.products});
      },
      error: error => console.error(error),
      complete: () => console.log('completed')
    });
  }
}
