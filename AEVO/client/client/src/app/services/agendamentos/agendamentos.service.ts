import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AgendamentosService {
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

  getAgendamentosUsuario(obj: any): Observable<any> {
    const headers = this.createHeaders();
    return this.http.post<any>(`${this.baseUrl}/agendamentos`, obj, {
      headers,
    });
  }

  getAgendamentosPorSala(salaId: string): Observable<any> {
    const headers = this.createHeaders();
    return this.http.get<any>(`${this.baseUrl}/agendamentos/salas/${salaId}`, {
      headers,
    });
  }

  getAgendamento(agendamentoId: string): Observable<any> {
    const headers = this.createHeaders();
    return this.http.get<any>(`${this.baseUrl}/agendamentos/${agendamentoId}`, {
      headers,
    });
  }

  createAgendamento(salaId: string, agendamento: any): Observable<any> {
    const headers = this.createHeaders();
    return this.http.post<any>(
      `${this.baseUrl}/agendamentos/salas/${salaId}`,
      agendamento,
      {
        headers,
      }
    );
  }

  updateAgendamento(agendamentoId: string, agendamento: any): Observable<any> {
    const headers = this.createHeaders();
    return this.http.put<any>(
      `${this.baseUrl}/agendamentos/${agendamentoId}`,
      agendamento,
      {
        headers,
      }
    );
  }

  deleteAgendamento(agendamentoId: string, usuarioId: string): Observable<any> {
    const headers = this.createHeaders();
    return this.http.delete<any>(
      `${this.baseUrl}/agendamentos/${usuarioId}/${agendamentoId}`,
      {
        headers,
      }
    );
  }
}
