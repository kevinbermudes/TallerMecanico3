import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cliente } from '../Entity/Cliente';
import { CartaPago } from '../Entity/CartaPago';
import { Factura } from '../Entity/Factura';
import { Notificacion } from '../Entity/Notificacion';
import { Pago } from '../Entity/Pago';
import { Servicio } from '../Entity/Servicio';
import { Vehiculo } from '../Entity/Vehiculo';
import {environment} from '../../env/environment';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {
  // private apiUrl = 'http://localhost:5132/api/Cliente';
  private apiUrl = `${environment.apiUrl}Cliente`;

  constructor(private http: HttpClient) {}

  // Obtener todos los clientes
  getAllClientes(): Observable<Cliente[]> {
    return this.http.get<Cliente[]>(`${this.apiUrl}`);
  }

  // Obtener un cliente por ID
  getClienteById(id: number): Observable<Cliente> {
    return this.http.get<Cliente>(`${this.apiUrl}/${id}`);
  }

  // Crear un nuevo cliente
  createCliente(cliente: Cliente): Observable<Cliente> {
    return this.http.post<Cliente>(`${this.apiUrl}`, cliente);
  }

  // Actualizar un cliente existente
  updateCliente(id: number, cliente: Cliente): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, cliente);
  }

  // Eliminar un cliente (borrado lógico)
  deleteCliente(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Obtener cartas de pago por ClienteId
  getCartasPagoByClienteId(id: number): Observable<CartaPago[]> {
    return this.http.get<CartaPago[]>(`${this.apiUrl}/${id}/cartas-pago`);
  }

  // Obtener facturas por ClienteId
  getFacturasByClienteId(id: number): Observable<Factura[]> {
    return this.http.get<Factura[]>(`${this.apiUrl}/${id}/facturas`);
  }

  // Obtener notificaciones por ClienteId
  getNotificacionesByClienteId(id: number): Observable<Notificacion[]> {
    return this.http.get<Notificacion[]>(`${this.apiUrl}/${id}/notificaciones`);
  }

  // Obtener pagos por ClienteId
  getPagosByClienteId(id: number): Observable<Pago[]> {
    return this.http.get<Pago[]>(`${this.apiUrl}/${id}/pagos`);
  }

  // Obtener servicios por ClienteId
  getServiciosByClienteId(id: number): Observable<Servicio[]> {
    return this.http.get<Servicio[]>(`${this.apiUrl}/${id}/servicios`);
  }

  // Obtener vehículos por ClienteId
  getVehiculosByClienteId(id: number): Observable<Vehiculo[]> {
    return this.http.get<Vehiculo[]>(`${this.apiUrl}/${id}/vehiculos`);
  }
  reactivarCliente(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/reactivar`, null);
  }

}
