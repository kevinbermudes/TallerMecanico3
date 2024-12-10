import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Factura } from '../Entity/Factura';
import {environment} from '../../env/environment';

@Injectable({
  providedIn: 'root',
})
export class FacturaService {
  // private apiUrl = 'http://localhost:5132/api/Factura';
  private apiUrl = `${environment.apiUrl}Factura`;

  constructor(private http: HttpClient) {}

  // Obtener todas las facturas
  getAllFacturas(): Observable<Factura[]> {
    return this.http.get<Factura[]>(this.apiUrl);
  }

  // Obtener una factura específica por ID
  getFacturaById(id: number): Observable<Factura> {
    return this.http.get<Factura>(`${this.apiUrl}/${id}`);
  }

  // Obtener facturas por cliente
  getFacturasByClienteId(clienteId: number): Observable<Factura[]> {
    return this.http.get<Factura[]>(`${this.apiUrl}/cliente/${clienteId}`);
  }

  // Crear una nueva factura
    createFactura(factura: Factura): Observable<Factura> {
    return this.http.post<Factura>(this.apiUrl, factura);
  }

  // Actualizar una factura existente
  updateFactura(id: number, factura: Factura): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, factura);
  }

  // Borrado lógico de una factura
  deleteFactura(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Obtener facturas por estado
  getFacturasByEstado(estado: string): Observable<Factura[]> {
    return this.http.get<Factura[]>(`${this.apiUrl}/estado/${estado}`);
  }
}
