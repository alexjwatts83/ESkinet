import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../shared/models/products';
import { CurrencyPipe, JsonPipe } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatDivider } from '@angular/material/divider';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [JsonPipe, CurrencyPipe, MatIcon, MatButton, MatFormField, MatInput, MatLabel, MatDivider],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  private shopService = inject(ShopService);
  private activatedRoute = inject(ActivatedRoute);

  product?: Product;

  ngOnInit(): void {
    this.loadProduct();
  }

  private loadProduct() {
    const productId = this.activatedRoute.snapshot.paramMap.get('id');
    console.log({productId});
    if (!productId)
      return;

    this.shopService.getProduct(productId).subscribe({
      next: product => {
        this.product = product;
        console.log({product});
      },
      error: error => console.error(error)
    })
  }
}
