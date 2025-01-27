import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  loading = false;
  busyRequestCount = 0;

  busy() {
    this.busyRequestCount++;
    this.loading = true;
    // this.spinnerService.show(undefined, {
    //   type: 'timer',
    //   bdColor: 'rgba(255,255,255,0.7)',
    //   color: '#333333'
    // });
  }

  idle() {
    this.busyRequestCount--;

    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.loading = false;
      // this.spinnerService.hide();
    }
  }
}
