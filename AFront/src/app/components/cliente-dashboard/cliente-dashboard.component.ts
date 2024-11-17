import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Service/auth.service';
import { Router } from '@angular/router';
import { MenubarModule } from 'primeng/menubar';
import { Button, ButtonDirective } from 'primeng/button';
import { MenuItem } from 'primeng/api';
import { NotificacionService } from '../../Service/notificacion.service';
import { SignalRService } from '../../Service/signal-r.service';
import { NgClass, CommonModule } from '@angular/common';

@Component({
  selector: 'app-cliente-dashboard',
  standalone: true,
  imports: [
    MenubarModule,
    Button,
    ButtonDirective,
    NgClass,
    CommonModule
  ],
  templateUrl: './cliente-dashboard.component.html',
  styleUrls: ['./cliente-dashboard.component.css']
})
export class ClienteDashboardComponent implements OnInit {
  clienteNombre: string = '';
  clienteEmail: string = '';
  clienteId: number | null = null;
  tieneNotificacionesCarrito: boolean = false;
  items: MenuItem[] = [];

  constructor(
    private signalRService: SignalRService,
    private authService: AuthService,
    private notificacionService: NotificacionService,
    private router: Router
  ) {}

  ngOnInit() {
    // Iniciar conexión con SignalR
    this.signalRService.startConnection();

    // Escuchar notificaciones en tiempo real
    this.signalRService.onReceiveNotification((message) => {
      console.log('Notificación recibida:', message);
      this.tieneNotificacionesCarrito = true; // Mostrar indicador
      this.generarItems(); // Actualizar el menú
    });

    // Obtener datos del cliente
    const clienteData = this.authService.getClienteData();
    if (clienteData) {
      this.clienteNombre = clienteData.nombre;
      this.clienteEmail = clienteData.email;
      this.clienteId = clienteData.id ? +clienteData.id : null;
    }

    // Verificar notificaciones iniciales
    this.verificarNotificacionesCarrito();

    // Generar ítems del menú
    this.generarItems();
  }

  verificarNotificacionesCarrito() {
    if (this.clienteId) {
      this.notificacionService.getNotificacionesCliente(this.clienteId).subscribe((response: any) => {
        const notificaciones = response.$values || response;

        // Verifica si hay notificaciones no leídas del carrito
        if (Array.isArray(notificaciones)) {
          this.tieneNotificacionesCarrito = notificaciones.some(
            (notificacion) => notificacion.tipo === 'Carrito' && !notificacion.leido
          );
          this.generarItems(); // Actualizamos los ítems del menú
        } else {
          console.error('Las notificaciones no son un array:', notificaciones);
        }
      });
    }
  }

  generarItems() {
    this.items = [
      {
        label: 'Tienda',
        icon: 'pi pi-store',
        command: () => this.navigateTo('/cliente/tienda')
      },
      {
        label: 'Cuentas',
        icon: 'pi pi-file',
        items: [
          { label: 'Facturas', icon: 'pi pi-file', command: () => this.navigateTo('/cliente/facturas') },
          { label: 'Cartas de Pago', icon: 'pi pi-envelope', command: () => this.navigateTo('/cliente/cartas-de-pago') }
        ]
      },
      {
        label: 'Vehículos',
        icon: 'pi pi-car',
        command: () => this.navigateTo('/cliente/vehiculos')
      },
      {
        label: 'Servicios',
        icon: 'pi pi-briefcase',
        command: () => this.navigateTo('/cliente/servicios')
      },
      {
        label: 'Carrito',
        icon: this.tieneNotificacionesCarrito ? 'pi pi-shopping-cart p-text-danger' : 'pi pi-shopping-cart',
        command: () => this.irAlCarrito()
      },
      {
        label: 'Perfil',
        icon: 'pi pi-user',
        command: () => this.navigateTo('/cliente/perfil')
      }
    ];
  }

  irAlCarrito() {
    if (this.clienteId) {
      this.notificacionService.getNotificacionesCliente(this.clienteId).subscribe((response: any) => {
        const notificaciones = response.$values || response;

        if (Array.isArray(notificaciones)) {
          const carritoNotificaciones = notificaciones.filter((n) => n.tipo === 'Carrito' && !n.leido);

          // Marcar las notificaciones como leídas
          const marcarLeidoRequests = carritoNotificaciones.map((notificacion) =>
            this.notificacionService.marcarComoLeido(notificacion.id)
          );

          Promise.all(marcarLeidoRequests).then(() => {
            this.verificarNotificacionesCarrito();
          });
        } else {
          console.error('Las notificaciones no son un array:', notificaciones);
        }
      });
    }

    // Deshabilitar el indicador de notificaciones
    this.tieneNotificacionesCarrito = false;
    this.router.navigate(['/cliente/carrito']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  navigateTo(route: string) {
    this.router.navigate([route]);
  }
}
