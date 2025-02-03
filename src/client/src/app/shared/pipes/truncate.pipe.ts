import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncate',
  standalone: true,
})
export class TruncatePipe implements PipeTransform {
  transform(value: string, limit: number): unknown {
    return value.length < limit ? value : value.slice(0, limit) + '...';
  }
}
