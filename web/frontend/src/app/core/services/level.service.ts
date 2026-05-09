import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class Level {
  
  private api = 'https://web-game-0p9u.onrender.com/levels';

  constructor(private http:HttpClient) {}

  getLevels() {
    return this.http.get<{id: number, name: string}[]>(this.api);
  }
  
}
