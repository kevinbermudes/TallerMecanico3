import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Carrito } from '../Entity/Carrito';

@Injectable({
  providedIn: 'root'
})
export class CarritoService {
  private apiUrl = 'http://localhost:5132/api/Carrito';

  constructor(private http: HttpClient) {}

  // Agregar producto al carrito
  agregarAlCarrito(data: { clienteId: number; productoId: number; cantidad: number }): Observable<Carrito> {
    return this.http.post<Carrito>(`${this.apiUrl}/agregar-al-carrito`, data);
  }

  // Obtener todos los carritos
  getAllCarritos(): Observable<Carrito[]> {
    return this.http.get<Carrito[]>(`${this.apiUrl}`);
  }

  // Obtener un carrito específico por ID
  getCarritoById(id: number): Observable<Carrito> {
    return this.http.get<Carrito>(`${this.apiUrl}/${id}`);
  }

  // Crear un nuevo carrito
  createCarrito(carrito: Carrito): Observable<Carrito> {
    return this.http.post<Carrito>(this.apiUrl, carrito);
  }

  // Actualizar un carrito existente
  updateCarrito(id: number, carrito: Carrito): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, carrito);
  }

  // Eliminar (borrado lógico) un carrito
  deleteCarrito(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Obtener carritos por cliente
  getCarritosByClienteId(clienteId: number): Observable<{ $values?: Carrito[] } | Carrito[]> {
    return this.http.get<{ $values?: Carrito[] } | Carrito[]>(`${this.apiUrl}/cliente/${clienteId}`);
  }

  // Obtener carritos por producto
  getCarritosByProductoId(productoId: number): Observable<Carrito[]> {
    return this.http.get<Carrito[]>(`${this.apiUrl}/producto/${productoId}`);
  }
}
