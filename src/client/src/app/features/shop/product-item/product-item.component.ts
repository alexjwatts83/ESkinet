import { Component, inject, Input } from '@angular/core';
import { Product } from '../../../shared/models/products';
import { CurrencyPipe, NgIf } from '@angular/common';
import { MatCard, MatCardContent, MatCardActions, MatCardImage } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-product-item',
  standalone: true,
  imports: [NgIf, MatCard, MatCardContent, CurrencyPipe, MatCardActions, MatIcon, MatButton, RouterLink, MatCardImage],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.scss'
})
export class ProductItemComponent {
  @Input() product?: Product;

  private cartService = inject(CartService);

  onAddItemToCart(item: Product, quantity = 1) {
    console.log({item, quantity});
    this.cartService.addItemToCart(item, quantity);
  }
}
