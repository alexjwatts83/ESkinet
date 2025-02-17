import {
  AfterViewInit,
  Component,
  inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { Order } from '../../shared/models/orders.models';
import { AdminService } from '../../core/services/admin.service';
import { OrderParams } from '../../shared/models/shop-params';
import { Pagination } from '../../shared/models/pagination';
import { MatIcon } from '@angular/material/icon';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { CurrencyPipe, DatePipe, NgFor, NgIf } from '@angular/common';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTabsModule } from '@angular/material/tabs';
import { TruncatePipe } from '../../shared/pipes/truncate.pipe';
import { RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { DialogService } from '../../core/services/dialog.service';
import { SnackbarService } from '../../core/services/snackbar.service';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatPaginatorModule,
    MatIcon,
    MatSelectModule,
    DatePipe,
    CurrencyPipe,
    MatLabel,
    MatTooltipModule,
    MatTabsModule,
    NgFor,
    TruncatePipe,
    RouterLink,
    MatButton,
  ],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss',
})
export class AdminComponent implements OnInit {
  private adminService = inject(AdminService);
  private dialogService = inject(DialogService);
  private snack = inject(SnackbarService);

  orderParams = new OrderParams();

  displayedColumns: string[] = [
    'id',
    'buyerEmail',
    'orderDate',
    'status',
    'total',
    'actions',
  ];
  dataSource = new MatTableDataSource<Order>([]);
  totalItems = 0;
  statusOptions = [
    'All',
    'Pending',
    'PaymentReceived',
    'PaymentFailed',
    'PaymentMismatch',
    'Refunded',
  ];

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders() {
    this.adminService.getOrders(this.orderParams).subscribe({
      next: (response: Pagination<Order>) => {
        console.log({ response });
        if (response.data) {
          this.dataSource.data = response.data;
          this.totalItems = response.count;
        } else {
          this.dataSource.data = [];
          this.totalItems = 0;
        }
      },
    });
  }

  onPageChange(event: PageEvent) {
    this.orderParams.pageNumber = event.pageIndex + 1;
    this.orderParams.pageSize = event.pageSize;
    this.loadOrders();
  }

  onFilterSelect(event: MatSelectChange) {
    this.orderParams.status = event.value;
    this.orderParams.pageNumber = 1;
    this.loadOrders();
  }

  async confirmRefund(id: string) {
    const confirm = await this.dialogService.confirm(
      'Confirm Refund',
      `Are you sure you want to issue refund? This cannot be undone!`
    );

    if (confirm) this.refundOrder(id);
  }

  private refundOrder(id: string) {
    console.log({ refund: id });
    this.adminService.refundOrder(id).subscribe({
      next: (order) => {
        console.log({ refundedOrder: order });
        this.dataSource.data = this.dataSource.data.map((x) =>
          x.id == id ? order : x
        );
        this.snack.success('Order was refunded');
      },
    });
  }
}
