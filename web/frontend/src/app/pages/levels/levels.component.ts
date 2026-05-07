import { Component } from '@angular/core';
import { Navbar } from '../../shared/components/navbar/navbar.component';
import { Footer } from '../../shared/components/footer/footer.component';

@Component({
  selector: 'app-levels',
  imports: [Navbar, Footer],
  templateUrl: './levels.component.html',
  styleUrl: './levels.component.scss',
})
export class Levels {

}
