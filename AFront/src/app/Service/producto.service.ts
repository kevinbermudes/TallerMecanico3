// producto.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Producto } from '../Entity/Producto';

@Injectable({
  providedIn: 'root'
})
export class ProductoService {
  private apiUrl = 'http://localhost:5132/api/Producto';

  constructor(private http: HttpClient) {}

  getProductos(): Observable<Producto[]> {
    return this.http.get<{ $values: Producto[] }>(this.apiUrl).pipe(
      map(response =>
        response.$values.map((producto) => ({
          ...producto,
          carritos: Array.isArray(producto.carritos) ? producto.carritos : producto.carritos?.$values || []
        }))
      )
    );
  }

  getProductoById(id: number): Observable<Producto> {
    return this.http.get<Producto>(`${this.apiUrl}/${id}`);
  }

  getImagenUrl(id: number): Observable<{ Url: string }> {
    return this.http.get<{ Url: string }>(`${this.apiUrl}/${id}/imagen`);
  }

  // Nuevo método para subir una imagen
  uploadImagen(id: number, imagen: File): Observable<{ Url: string }> {
    const formData = new FormData();
    formData.append('imagen', imagen, imagen.name);
    return this.http.post<{ Url: string }>(`${this.apiUrl}/${id}/imagen`, formData);
  }
}
