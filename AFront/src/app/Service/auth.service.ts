import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import {Router} from '@angular/router';
import {environment} from '../../env/environment';

interface DecodedToken {
  sub: string;
  email: string;
  nombre: string;
  role: string;
  exp: number;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // private apiUrl = 'http://localhost:5132/api/Auth';
  private apiUrl = `${environment.apiUrl}Auth`;

  private isBrowser: boolean;

  constructor(@Inject(PLATFORM_ID) private platformId: Object, private http: HttpClient,  private router: Router) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  login(email: string, password: string): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, { email, password });
  }

  registrar(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/registro`, data);
  }

  setToken(token: string): void {
    if (this.isBrowser) {
      console.log("Almacenando token:", token);
      localStorage.setItem('token', token);
    }
  }

  getToken(): string | null {
    const token = this.isBrowser ? localStorage.getItem('token') : null;
    console.log("Token recuperado:", token);
    return token;
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) {
      return false;
    }
    try {
      const decoded: DecodedToken = jwtDecode(token);
      return decoded.exp > Date.now() / 1000;
    } catch (error) {
      console.error('Error al decodificar el token o token expirado:', error);
      return false;
    }
  }

  getUserRole(): string | null {
    const token = this.getToken();
    if (!token || token.split('.').length !== 3) {
      console.error('Token no válido o malformado:', token);
      return null;
    }
    try {
      const decoded: DecodedToken = jwtDecode(token);
      return decoded.role;
    } catch (error) {
      console.error('Error al decodificar el token:', error);
      return null;
    }
  }

  logout(): void {
    if (this.isBrowser) {
      localStorage.removeItem('token');
      this.router.navigate(['/login']);
    }
  }

  getClienteData(): { id: number; nombre: string; email: string } | null {
    const token = this.getToken();
    if (!token) {
      return null;
    }
    try {
      const decoded: DecodedToken = jwtDecode(token);
      return {
        id: parseInt(decoded.sub, 10),
        nombre: decoded.nombre,
        email: decoded.email
      };
    } catch (error) {
      console.error('Error al decodificar el token:', error);
      return null;
    }
  }
  isAdmin(): boolean {
    const role = this.getUserRole();
    return role === 'Admin';
  }
  getAdminData(): { id: number; nombre: string; email: string } | null {
    if (!this.isAdmin()) {
      console.warn('El usuario no tiene permisos de administrador.');
      return null;
    }

    const token = this.getToken();
    if (!token) {
      return null;
    }
    try {
      const decoded: DecodedToken = jwtDecode(token);
      return {
        id: parseInt(decoded.sub, 10),
        nombre: decoded.nombre,
        email: decoded.email,
      };
    } catch (error) {
      console.error('Error al decodificar el token:', error);
      return null;
    }
  }
}
