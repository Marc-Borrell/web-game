import { Component, inject, OnInit } from '@angular/core';
import { Auth } from '../../core/services/auth.service';
import { Router } from '@angular/router';
import { Navbar } from '../../shared/components/navbar/navbar.component';
import { Footer } from '../../shared/components/footer/footer.component';

@Component({
  selector: 'app-home',
  imports: [Navbar, Footer],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class Home {


  private authService = inject(Auth);
  private router = inject(Router);
  public usuari: String = this.authService.getUser().name;



}
