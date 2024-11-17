import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {map, Observable} from 'rxjs';
import {Notificacion} from '../Entity/Notificacion';

@Injectable({
  providedIn: 'root'
})
export class NotificacionService {
  private apiUrl = 'http://localhost:5132/api/Notificacion';

  constructor(private http: HttpClient) {}

  getNotificacionesCliente(clienteId: number): Observable<Notificacion[]> {
    return this.http.get<{ $values: Notificacion[] }>(`${this.apiUrl}/cliente/${clienteId}`).pipe(
      map((response) => response.$values || [])
    );
  }


  marcarComoLeido(notificacionId: number): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${notificacionId}/marcar-como-leido`, {});
  }
}
