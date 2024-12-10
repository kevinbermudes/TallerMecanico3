import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Producto } from '../Entity/Producto';
import {environment} from '../../env/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductoService {
  // private apiUrl = 'http://localhost:5132/api/Producto';
  private apiUrl = `${environment.apiUrl}Producto`;

  constructor(private http: HttpClient) {}

  // Obtener todos los productos
  getProductos(): Observable<Producto[]> {
    return this.http.get<Producto[]>(`${this.apiUrl}`);
  }

  // Obtener un producto específico por ID
  getProductoById(id: number): Observable<Producto> {
    return this.http.get<Producto>(`${this.apiUrl}/${id}`);
  }

  // Crear un nuevo producto
  createProducto(producto: FormData): Observable<Producto> {
    return this.http.post<Producto>(`${this.apiUrl}`, producto);
  }

  // Actualizar un producto existente
  updateProducto(id: number, producto: FormData): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, producto);
  }

  // Eliminar (borrado lógico) un producto
  deleteProducto(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Obtener URL de la imagen de un producto
  getImagenUrl(id: number): Observable<{ Url: string }> {
    return this.http.get<{ Url: string }>(`${this.apiUrl}/${id}/imagen`);
  }

  // Subir imagen para un producto
  uploadImagen(id: number, imagen: File): Observable<{ Url: string }> {
    const formData = new FormData();
    formData.append('imagen', imagen, imagen.name);
    return this.http.post<{ Url: string }>(`${this.apiUrl}/${id}/imagen`, formData);
  }

  // Obtener productos por categoría
  getProductosByCategoria(categoria: number): Observable<Producto[]> {
    return this.http.get<Producto[]>(`${this.apiUrl}/categoria/${categoria}`);
  }

  // Actualizar stock de un producto
  updateStock(id: number, stock: number): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/stock`, { stock });
  }
  reactivarProducto(id: number): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/reactivar`, {});
  }

}
