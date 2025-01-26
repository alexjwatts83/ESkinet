import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/products';
import { NgFor } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { ProductItemComponent } from './product-item/product-item.component';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [NgFor, ProductItemComponent, MatButton, MatIcon],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);

  products: Product[] = [];
  selectTypes: string[] = [];
  selectedBrands: string[] = [];

  ngOnInit(): void {
    console.log('ngOnInit');
    this.initShop();
  }

  private initShop() {
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.shopService.getProducts().subscribe({
      next: (x) => {
        this.products = x.data;
        console.log({ prod: this.products });
      },
      error: (error) => console.error(error),
      complete: () => console.log('completed'),
    });
  }

  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands : this.selectedBrands,
        selectTypes: this.selectTypes
      }
    });
    dialogRef.afterClosed().subscribe(x => {
      console.log({x});
      if (x) {
        this.selectTypes = x.selectTypes;
        this.selectedBrands = x.selectedBrands;
        this.shopService.getProducts(this.selectedBrands, this.selectTypes).subscribe({
          next: (x) => {
            this.products = x.data;
            console.log({ prod: this.products });
          },
          error: (error) => console.error(error),
          complete: () => console.log('completed'),
        });
      }
    })
  }
}
