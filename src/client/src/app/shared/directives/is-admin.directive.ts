import {
  Directive,
  effect,
  inject,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { AccountsService } from '../../core/services/accounts.service';

@Directive({
  selector: '[appIsAdmin]',
  standalone: true,
})
export class IsAdminDirective {
  private accountsService = inject(AccountsService);
  private viewContainerRef = inject(ViewContainerRef);
  private templateRef = inject(TemplateRef);

  constructor() {
    effect(() => {
      if (this.accountsService.isAdmin()) {
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      } else {
        this.viewContainerRef.clear();
      }
    })
  }
}
