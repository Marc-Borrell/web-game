import { Component } from '@angular/core';
import { Routes } from '@angular/router';
import { Register } from './pages/register/register';
import { Login } from './pages/login/login.component';
import { Home } from './pages/home/home.component';
import { Levels } from './pages/levels/levels.component';
import { Ranking } from './pages/ranking/ranking.component';
import { App } from './app';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {path: 'register', component: Register},
    {path: 'login',    component: Login },

    {path: 'home', component: Home,  canActivate: [authGuard]},
    {path: 'levels', component: Levels, canActivate: [authGuard]},
    {path: 'rankings', component: Ranking, canActivate: [authGuard] },

    //{path: 'main', component: App },

    { path: '',   redirectTo: 'login', pathMatch: 'full' },
     { path: '**', redirectTo: 'login' }


];
