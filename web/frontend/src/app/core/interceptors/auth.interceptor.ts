import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Auth } from '../services/auth.service';
import { Router } from '@angular/router';
import { catchError } from 'rxjs';
import { throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  //return next(req);
  const authService = inject(Auth);
  const router = inject(Router);
  const token = authService.getToken();

  if(req.url.includes('/users/login') || req.url.includes('/users/register')) {
    return next(req);
  }

  if (!token) {
    router.navigate(['/login']);
    return throwError(() => new Error("No s'ha trobat el token d'autentificació "));
  }

  req = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  return next(req).pipe(
    catchError((error) => {
      if (error.status === 401) {
        authService.logout();
        router.navigate(['/login']);
        return throwError(() => new Error('Sessió caducada. Si us plau inicia sessió de nou.'));
      }
      return throwError(() => error);
    })
  )
};
