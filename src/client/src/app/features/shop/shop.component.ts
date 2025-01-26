import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/products';
import { NgFor } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { ProductItemComponent } from './product-item/product-item.component';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import {
  MatSelectionList,
  MatListOption,
  MatSelectionListChange,
} from '@angular/material/list';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    NgFor,
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatMenuTrigger,
    MatSelectionList,
    MatListOption,
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);

  products: Product[] = [];
  selectTypes: string[] = [];
  selectedBrands: string[] = [];
  selectedSort: string = 'name';
  sortOptions = [
    {
      name: 'Alphabetically',
      value: 'name',
    },
    {
      name: 'Price: Low-High',
      value: 'priceAsc',
    },
    {
      name: 'Price: High-Low',
      value: 'priceDesc',
    },
  ];

  ngOnInit(): void {
    console.log('ngOnInit');
    this.initShop();
  }

  private initShop() {
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.getProducts();
  }

  private getProducts() {
    this.shopService
      .getProducts(this.selectedBrands, this.selectTypes, this.selectedSort)
      .subscribe({
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
        selectedBrands: this.selectedBrands,
        selectTypes: this.selectTypes,
      },
    });
    dialogRef.afterClosed().subscribe((x) => {
      console.log({ x });
      if (x) {
        this.selectTypes = x.selectTypes;
        this.selectedBrands = x.selectedBrands;
        console.log({ selectTypes: this.selectTypes, selectedBrands: this.selectedBrands });
        
        this.getProducts();
      }
    });
  }

  onSortChanged($event: MatSelectionListChange) {
    const selectedOption = $event.options[0];

    if (selectedOption) {
      this.selectedSort = selectedOption.value;
      console.log({ sort: this.selectedSort });
      this.getProducts();
    }
  }
}
