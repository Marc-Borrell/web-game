import { Component, inject } from '@angular/core';
import { Routes } from '@angular/router';
import { Register } from './pages/register/register';
import { Login } from './pages/login/login.component';
import { Home } from './pages/home/home.component';
import { Levels } from './pages/levels/levels.component';
import { Ranking } from './pages/ranking/ranking.component';
import { App } from './app';
import { authGuard } from './core/guards/auth.guard';
import { Auth } from './core/services/auth.service';
import { Start } from './pages/start/start';

export const routes: Routes = [

    {path: 'start', component: Start},
    {path: 'register', component: Register},
    {path: 'login',    component: Login },

    {path: 'home', component: Home,  canActivate: [authGuard]},
    {path: 'levels', component: Levels, canActivate: [authGuard]},
    {path: 'rankings', component: Ranking, canActivate: [authGuard] },

    //{path: 'main', component: App },

    { path: '',   redirectTo: () => {
        const authService = inject(Auth);
        return authService.isAuthenticated() ? '/home' : '/start';
    }, pathMatch: 'full' },
     { path: '**', redirectTo: 'start' }


];
