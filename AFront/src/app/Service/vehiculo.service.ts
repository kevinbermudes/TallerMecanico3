import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Vehiculo } from '../Entity/Vehiculo';
import {environment} from '../../env/environment';

@Injectable({
  providedIn: 'root',
})
export class VehiculoService {
  // private apiUrl = 'http://localhost:5132/api/Vehiculo';
  private apiUrl = `${environment.apiUrl}Vehiculo`;
  constructor(private http: HttpClient) {}

  getVehiculosByClienteId(clienteId: number): Observable<Vehiculo[]> {
    return this.http.get<Vehiculo[]>(`${this.apiUrl}/cliente/${clienteId}`);
  }

  createVehiculo(vehiculo: Vehiculo): Observable<Vehiculo> {
    return this.http.post<Vehiculo>(this.apiUrl, vehiculo);
  }

  deleteVehiculo(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
