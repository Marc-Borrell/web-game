import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import {FormsModule} from '@angular/forms';
import { Auth } from '../../../core/services/auth.service';
import { UnityService } from '../../../core/services/unity.service';

@Component({  
  selector: 'app-navbar',
  imports: [RouterLink, FormsModule, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class Navbar {
  email: string = '';
  password: string = '';
  error: string = '';
  loading: boolean = false;

  private router = inject(Router);
  private authService = inject(Auth);
  public usuari: String = this.authService.getUser().name; 

  constructor(private unityService: UnityService) {}

 logout() {
  const instance = this.unityService.getInstance();
  if (instance) {
    instance.Quit().then(() => {
      this.unityService.setInstance(null);
      this.authService.logout();
      window.location.href = '/login';
    });
  } else {
    this.authService.logout();
    window.location.href = '/login';
  }
}

}
