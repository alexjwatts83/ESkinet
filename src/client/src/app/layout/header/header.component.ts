import { Component, inject } from '@angular/core';
import { MatBadge } from '@angular/material/badge';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { RouterLink, RouterLinkActive } from '@angular/router';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import { BusyService } from '../../core/services/busy.service';
import { CartService } from '../../core/services/cart.service';
@Component({
  selector: 'app-header',
  standalone: true,
  imports: [MatIcon, MatBadge, MatButton, RouterLink, RouterLinkActive,MatProgressBarModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {
  busyService = inject(BusyService);
  cartService = inject(CartService);
}
