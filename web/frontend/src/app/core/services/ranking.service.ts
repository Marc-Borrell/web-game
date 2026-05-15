import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export interface RankingEntry {
  name: string;
  moves: number;
  time_ms: number;
}

@Injectable({
  providedIn: 'root',
})
export class RankingService {
  private apiBase = environment.apiUrl + '/ranking';

  constructor(private http: HttpClient) {}

  getRanking(level_id: number): Observable<RankingEntry[]> {
    return this.http.get<RankingEntry[]>(`${this.apiBase}?level_id=${level_id}`);
  }
}
