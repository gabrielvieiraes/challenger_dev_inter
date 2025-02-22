import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SalasService {
  private baseUrl = 'https://localhost:44359';

  constructor(private http: HttpClient) {}

  private createHeaders() {
    const accessToken = localStorage.getItem('accessToken');
    let headers = new HttpHeaders();

    if (accessToken) {
      headers = headers.set('Authorization', `Bearer ${accessToken}`);
    }

    return headers;
  }

  getSalas(): Observable<any> {
    const headers = this.createHeaders();
    return this.http.get<any>(`${this.baseUrl}/salas`, { headers });
  }

  getSala(salaId: string): Observable<any> {
    const headers = this.createHeaders();
    return this.http.get<any>(`${this.baseUrl}/salas/${salaId}`, {
      headers,
    });
  }

  createSala(obj: any): Observable<any> {
    const headers = this.createHeaders();
    return this.http.post<any>(`${this.baseUrl}/salas`, obj, {
      headers,
    });
  }

  updateSala(salaId: string, obj: any): Observable<any> {
    const headers = this.createHeaders();
    return this.http.put<any>(`${this.baseUrl}/salas/${salaId}`, obj, {
      headers,
    });
  }

  deleteSala(salaId: string): Observable<any> {
    const headers = this.createHeaders();
    return this.http.delete<any>(`${this.baseUrl}/salas/${salaId}`, {
      headers,
    });
  }
}
