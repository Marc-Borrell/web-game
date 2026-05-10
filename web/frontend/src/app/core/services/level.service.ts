import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class Level {
  
  private api = environment.apiUrl + '/levels';

  constructor(private http:HttpClient) {}

  getLevels() {
    return this.http.get<{id: number, name: string}[]>(this.api);
  }
  
}
