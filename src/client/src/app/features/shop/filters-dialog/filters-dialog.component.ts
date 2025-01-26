import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { MatDivider } from '@angular/material/divider';
import { MatSelectionList, MatListOption } from '@angular/material/list';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filters-dialog',
  standalone: true,
  imports: [
    MatDivider,
    MatSelectionList,
    MatListOption,
    MatButton,
    FormsModule,
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss',
})
export class FiltersDialogComponent implements OnInit {
  shopService = inject(ShopService);
  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  data = inject(MAT_DIALOG_DATA);

  selectTypes: string[] = this.data.selectTypes;
  selectedBrands: string[] = this.data.selectedBrands;

  ngOnInit(): void {
    console.log({ data: this.data });
  }

  applyFilters() {
    this.dialogRef.close({
      selectTypes: this.selectTypes,
      selectedBrands: this.selectedBrands,
    });
  }
  
  onNoClick() {
    this.dialogRef.close();
  }
}
