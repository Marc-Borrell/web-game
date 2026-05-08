import { Component, inject, signal } from '@angular/core';
import { NavigationStart, Router, RouterOutlet } from '@angular/router';
//import { Navbar } from './shared/components/navbar/navbar.component';
//import { Footer } from './shared/components/footer/footer.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('frontend');
  private router = inject(Router);
  private navigationHistory: string[] = [];

   constructor() {

    this.router.events.subscribe((event) => {

      if (event instanceof NavigationStart) {

        const isNavigatingBack =
          this.navigationHistory.includes(event.url);

        document.documentElement.classList.toggle(
          'back',
          isNavigatingBack
        );

        document.documentElement.classList.toggle(
          'forward',
          !isNavigatingBack
        );

        if (!isNavigatingBack) {

          this.navigationHistory.push(event.url);

        } else {

          const index =
            this.navigationHistory.indexOf(event.url);

          this.navigationHistory =
            this.navigationHistory.slice(0, index + 1);
        }
      }
    });
  }
}

