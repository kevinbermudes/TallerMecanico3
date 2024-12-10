import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenubarModule } from 'primeng/menubar';
import { MenuItem } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../Service/auth.service';
import { SignalRService } from '../../Service/signal-r.service';
import { ToastModule } from 'primeng/toast';
import {NgClass} from '@angular/common';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [MenubarModule, ButtonModule, ToastModule, NgClass],
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  adminNombre: string = '';
  adminEmail: string = '';
  items: MenuItem[] = [];
  isDarkMode: boolean = false;
  constructor(
    private authService: AuthService,
    private signalRService: SignalRService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Iniciar SignalR para el administrador
    this.signalRService.startConnection();

    // Obtener datos del administrador
    const adminData = this.authService.getAdminData();
    if (adminData) {
      this.adminNombre = adminData.nombre;
      this.adminEmail = adminData.email;
    }

    // Configurar los ítems del menú
    this.generarItems();
  }

  generarItems(): void {
    this.items = [
      {
        label: 'Gestión de Usuarios',
        icon: 'pi pi-users',
        command: () => this.navigateTo('/admin/clientes')
      },
      {
        label: 'Gestión de Facturas',
        icon: 'pi pi-file',
        command: () => this.navigateTo('/admin/facturas')
      },
      {
        label: 'Gestión de Servicios',
        icon: 'pi pi-briefcase',
        command: () => this.navigateTo('/admin/servicios')
      },
      {
        label: 'Gestión de Productos',
        icon: 'pi pi-box',
        command: () => this.navigateTo('/admin/productos')
      },
      {
        label: 'Reportes',
        icon: 'pi pi-chart-bar',
        command: () => this.navigateTo('/admin/reportes')
      },
      {
        label: 'Cerrar Sesión',
        icon: 'pi pi-sign-out',
        command: () => this.logout()
      }
    ];
  }

  navigateTo(route: string): void {
    this.router.navigate([route]);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
  toggleDarkMode(): void {
    this.isDarkMode = !this.isDarkMode;
    // Aplica la clase de modo oscuro al body para estilos globales
    if (this.isDarkMode) {
      document.body.classList.add('dark-mode');
    } else {
      document.body.classList.remove('dark-mode');
    }
  }
}
