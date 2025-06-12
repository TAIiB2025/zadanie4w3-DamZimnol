import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Film } from '../models/film';
import { FilmBody } from '../models/film-body';

@Injectable({
  providedIn: 'root'
})
export class ListaService {
  private readonly apiUrl = 'http://localhost:5005/api/film';

  constructor(private http: HttpClient) {}
  
  get(titleFilter?: string): Observable<Film[]> {
    let params = new HttpParams();
    if (titleFilter) {
      params = params.set('titleFilter', titleFilter);
    }
    return this.http.get<Film[]>(this.apiUrl, { params });
  }

  getByID(id: number): Observable<Film> {
    return this.http.get<Film>(`${this.apiUrl}/${id}`);
  }

  post(body: FilmBody): Observable<void> {
    return this.http.post<void>(this.apiUrl, body);
  }

  put(id: number, body: FilmBody): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, body);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
