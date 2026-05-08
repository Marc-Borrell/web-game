import { Component, inject, OnInit} from '@angular/core';
import {FormsModule} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Auth } from '../../core/services/auth.service';
import { OAuthService, AuthConfig } from 'angular-oauth2-oidc';
import { environment } from '../../../environments/environment';


const googleAuthConfig: AuthConfig = {
  issuer: 'https://accounts.google.com',
  strictDiscoveryDocumentValidation: false,
  clientId: environment.googleClientId,
  redirectUri: window.location.origin,
  scope: 'openid profile email',
};

@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class Login /*implements OnInit*/{
  email: string = '';
  password: string = '';
  error: string = '';
  loading: boolean = false;

  private router = inject(Router);
  private authService = inject(Auth);
  private oauthService = inject(OAuthService);

  onSubmit() {
    this.error = '';
    this.loading = true;

    this.authService.login({email: this.email, password: this.password}).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/home']);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.msg || 'Error al inciar sessió';
      }
    });
  }

   /*ngOnInit() {
    this.oauthService.configure(googleAuthConfig);
    this.oauthService.loadDiscoveryDocumentAndTryLogin().then(() => {
      if (this.oauthService.hasValidIdToken()) {
        const idToken = this.oauthService.getIdToken();
        this.authService.googleLogin(idToken).subscribe({
          next: () => this.router.navigate(['/home']),
          error: (err) => {
            console.error('Error Google login:', err);
            this.error = 'Error amb Google Login';
          }
        });
      }
    });
  }

  loginWithGoogle() {
    this.oauthService.initImplicitFlow();
  } */
}