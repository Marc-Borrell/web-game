import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'TimeFormatPipe',
  standalone: true,
})
export class TimeFormatPipe implements PipeTransform {

  transform(time_ms: number): string {
    const minuts = Math.floor((time_ms / 60000));
    const segons = Math.floor((time_ms % 60000) / 1000);
    return `${String(minuts).padStart(2, '0')}:${String(segons).padStart(2, '0')}`;
  }

}
