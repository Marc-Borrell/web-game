import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Login } from '../../pages/login/login.component';
// por si acaso
import { environment } from '../../../environments/environment';


export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  name: string;
}

export interface AuthResponse {
  msg: string;
  token: string;
  user: {
    id: number;
    name: string;
    email: string;
  }
}

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private baseURL = environment.apiUrl + '/users';

  constructor(private http: HttpClient){}

  login(request: LoginRequest): Observable<AuthResponse>{
    return this.http.post<AuthResponse>(`${this.baseURL}/login`, request).pipe(
      tap((response) => {
        if (response.token) {
          localStorage.setItem('auth_token', response.token);
          localStorage.setItem('auth_user', JSON.stringify(response.user));
        }
      })
    )
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseURL}/register`, request).pipe(
      tap((response) => {
        if (response.token) {
          localStorage.setItem('auth_token', response.token);
          localStorage.setItem('auth_user', JSON.stringify(response.user));
        }
      })
    )
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }
  
  getUser(): any {
    const user = localStorage.getItem('auth_user');
    return user ? JSON.parse(user) : null;
  }

  logout(): void {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('refresh_token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  googleLogin(googleToken: string): Observable<AuthResponse> {
  return this.http.post<AuthResponse>(`${this.baseURL}/google`, { googleToken }).pipe(
    tap((response) => {
      if (response.token) {
        localStorage.setItem('auth_token', response.token);
        localStorage.setItem('auth_user', JSON.stringify(response.user));
      }
    })
  );
}
}
