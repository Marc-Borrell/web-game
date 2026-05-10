import { Component, inject} from '@angular/core';
import {FormsModule} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Auth } from '../../core/services/auth.service';

@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  name: string = '';
  email: string = '';
  password: string = '';
  passwordVerify: string = '';
  error: string = '';
  loading: boolean = false;

  private router = inject(Router);
  private authService = inject(Auth);

  onSubmit() {
    this.error = '';

    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
    if (!passwordRegex.test(this.password)) {
    this.error = 'La contrasenya ha de tenir mínim 8 caràcters, una majúscula, una minúscula, un número i un caràcter especial (@$!%*?&)';
    return;
  }

    if (this.password !== this.passwordVerify) {
      this.error = 'Les contrasenyes no coincideixen';
       return;
    }

    this.loading = true;

    this.authService.register({
      name: this.name,
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/home']);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.msg || 'Error al registrar-se';
      }
    });
  }
}
