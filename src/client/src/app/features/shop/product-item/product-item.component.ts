import { Component, Input, OnInit } from '@angular/core';
import { Product } from '../../../shared/models/products';
import { CurrencyPipe, NgIf } from '@angular/common';
import { MatCard, MatCardContent, MatCardActions } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
@Component({
  selector: 'app-product-item',
  standalone: true,
  imports: [NgIf, MatCard, MatCardContent, CurrencyPipe, MatCardActions, MatIcon, MatButton],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.scss'
})
export class ProductItemComponent implements OnInit {
  @Input() product?: Product;

  ngOnInit(): void {
    console.log({prod: this.product});
  }
}
