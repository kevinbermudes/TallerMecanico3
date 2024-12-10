import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {CartaPago} from '../Entity/CartaPago';
import {Factura} from '../Entity/Factura';
import {environment} from '../../env/environment';

@Injectable({
  providedIn: 'root',
})
export class CartaPagoService {
  // private apiUrl = 'http://localhost:5132/api/cartaPago';
  private apiUrl = `${environment.apiUrl}cartaPago`;

  constructor(private http: HttpClient) {}

  getCartasPagoByClienteId(clienteId: number): Observable<CartaPago[]> {
    return this.http.get<CartaPago[]>(`${this.apiUrl}/cliente/${clienteId}`);
  }

  getFacturasByCartaPagoId(cartaPagoId: number): Observable<Factura[]> {
    return this.http.get<Factura[]>(`${this.apiUrl}/${cartaPagoId}/facturas`);
  }

}
