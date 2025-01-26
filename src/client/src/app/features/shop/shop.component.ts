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
import { ShopParams } from '../../shared/models/shop-params';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { FormsModule } from '@angular/forms';

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
    MatPaginator,
    FormsModule
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);

  products?: Pagination<Product>;
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

  shopParams = new ShopParams();
  pageSizeOptions = [5, 10, 15, 20, 50, 100];

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
    console.log({ shopParams: this.shopParams });
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (x) => {
        this.products = x;
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
        selectedBrands: this.shopParams.brands,
        selectTypes: this.shopParams.types,
      },
    });
    dialogRef.afterClosed().subscribe((x) => {
      console.log({ x });
      if (x) {
        this.shopParams.types = x.selectTypes;
        this.shopParams.brands = x.selectedBrands;
        this.shopParams.pageNumber = 1;
        this.getProducts();
      }
    });
  }

  onSortChanged($event: MatSelectionListChange) {
    const selectedOption = $event.options[0];

    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;
      this.shopParams.pageNumber = 1;
      this.getProducts();
    }
  }

  onClearClicked() {
    this.shopParams = new ShopParams();
    this.getProducts();
  }

  handlePageEvent($event: PageEvent) {
    this.shopParams.pageNumber = $event.pageIndex + 1;
    this.shopParams.pageSize = $event.pageSize;
    this.getProducts();
  }

  onSearchChanged() {
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }
}
