import { Component } from '@angular/core';
import { Navbar } from '../../shared/components/navbar/navbar.component';
import { Footer } from '../../shared/components/footer/footer.component';

@Component({
  selector: 'app-ranking',
  imports: [Navbar, Footer],
  templateUrl: './ranking.component.html',
  styleUrl: './ranking.component.scss',
})
export class Ranking {

}
