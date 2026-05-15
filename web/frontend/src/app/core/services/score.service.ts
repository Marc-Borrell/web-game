import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';

export interface ScorePayload {
  level_id: number;
  moves: number;
  time_ms: number;
}

@Injectable({
  providedIn: 'root',
})
export class Score {
  private apiBase = environment.apiUrl + '/score';

  constructor(private http: HttpClient) {}

  guardarScore(payload: ScorePayload): Observable<any> {
    const token = localStorage.getItem('auth_token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.post(this.apiBase, payload, {headers});
  }

  getScoreUser(): Observable<number[]> {
    const token = localStorage.getItem('auth_token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`});
    return this.http.get<number[]>(`${this.apiBase}/bloquejat`, {headers}).pipe(
      map(ids => ids.map(id => Number(id)))
    );
  }
}
