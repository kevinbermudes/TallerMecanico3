import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map, Observable} from 'rxjs';
import {Servicio} from '../Entity/Servicio';
import {environment} from '../../env/environment';

@Injectable({
  providedIn: 'root'
})
export class ServicioService {
  // private apiUrl = 'http://localhost:5132/api/Servicio';
  private apiUrl = `${environment.apiUrl}Servicio`;
  constructor(private http: HttpClient) {}

  getAllServicios(): Observable<Servicio[]> {
    return this.http.get<Servicio[]>(`${this.apiUrl}`);
  }

  getServiciosByClienteId(clienteId: number): Observable<Servicio[]> {
    return this.http.get<Servicio[]>(`${this.apiUrl}/cliente/${clienteId}`);
  }


  contratarServicio(servicioId: number, clienteId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/contratar`, { servicioId, clienteId });
  }
  uploadImagen(servicioId: number, imagen: File): Observable<any> {
    const formData = new FormData();
    formData.append('imagen', imagen);

    return this.http.post<any>(`${this.apiUrl}/${servicioId}/imagen`, formData);
  }
  quitarServicioDeCliente(servicioId: number, clienteId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${servicioId}/cliente/${clienteId}`);
  }
  obtenerImagen(servicioId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${servicioId}/imagen`);
  }


  getServicioById(id: number): Observable<Servicio> {
    return this.http.get<Servicio>(`${this.apiUrl}/${id}`);
  }

  createServicio(servicio: FormData): Observable<Servicio> {
    return this.http.post<Servicio>(this.apiUrl, servicio);
  }


  updateServicio(id: number, servicio: Servicio, imagen?: File): Observable<void> {
    const formData = new FormData();

    // Agrega los datos del servicio
    formData.append('id', servicio.id!.toString());
    formData.append('nombre', servicio.nombre!);
    formData.append('descripcion', servicio.descripcion!);
    formData.append('precio', servicio.precio!.toString());
    formData.append('imagen', servicio.imagen || '');

    // Si se proporciona una imagen, agrégala al FormData
    if (imagen) {
      formData.append('imagen', imagen);
    }

    return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
  }


  deleteServicio(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  reactivarServicio(id: number): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/reactivar`, null);
  }

}
