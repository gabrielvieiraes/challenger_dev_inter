import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = 'https://localhost:44359';

  constructor(private http: HttpClient) {}

  login(email: string, password: string, idioma: string): Observable<any> {
    const body = { email, password, idioma };
    return this.http.post(`${this.apiUrl}/usuarios/login`, body);
  }

  register(
    email: string,
    nome: string,
    idioma: string,
    fusoHorario: string,
    password: string
  ): Observable<any> {
    const body = { email, nome, idioma, fusoHorario, password };
    return this.http.post(`${this.apiUrl}/usuarios`, body);
  }
}
